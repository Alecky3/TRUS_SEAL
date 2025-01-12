using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Protocol;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Security.Claims;
using TrustSeal.Areas.Identity.Data;
using TrustSeal.Models;


namespace TrustSeal.Pages.SupportMessage
{
    [Authorize]
    public class IndexModel: PageModel {
        private readonly TrustSeal.Areas.Identity.Data.TSAuth _context;
        private readonly UserManager<TSUser> _userManager;
        private readonly ILogger<IndexModel> _logger;
        public IndexModel(TrustSeal.Areas.Identity.Data.TSAuth context,
         ILogger<IndexModel> logger,
         UserManager<TSUser> userManager)
         {
             _context = context;
            _logger = logger;
            _userManager = userManager;
         }

         public List<Support> SupportMessages {get;set;} = new List<Support>();
         public List<TSUser> UserWithTickets {get;set;} = new List<TSUser>();

         public async Task<IActionResult> OnGetAsync()
         {
            var user = await _userManager.GetUserAsync(User);
            if(user!=null)
            {
                var Roles = await _userManager.GetRolesAsync(user);
                if(Roles.Contains("Admin"))
                {
                    UserWithTickets = await _context.Users
                                                    .Include(u=>u.SupportTickets)
                                                    .ThenInclude(s=>s.SupportAttachments)
                                                    .Where(u=>u.SupportTickets.Count() > 0)
                                                    .ToListAsync();
                    return Page();
                    
                } else {
                    SupportMessages = await _context.SupportMessages
                                                    .Include(m=>m.SendBy)
                                                    .Include(m=>m.Replies)
                                                    .ThenInclude(s=>s.SendBy)
                                                    .Include(s=>s.SupportAttachments)
                                                    .Include(m=>m.SupportAttachments)
                                                    .Where(m=>m.SendById == user.Id)
                                                    .ToListAsync();
                    return Page();
                }
            }
            return NotFound();
         }
         public async Task<IActionResult> OnPostAsync()
         {
            var user = await _userManager.GetUserAsync(User);
            if(user!=null)
            {
                var fileIds = Request.Form.Keys.Where(k=>k=="fileId");
                var SupportMessage = new Support();
                SupportMessage.Message = Request.Form["message"];
                SupportMessage.Read = false;
                SupportMessage.SendById = user.Id;
                SupportMessage.TicketNumber = await GenerateSupportTicket();
                SupportMessage.CreatedAt = DateTime.Now;
                SupportMessage.UpdatedAt = DateTime.Now;

                _context.SupportMessages.Add(SupportMessage);
                await _context.SaveChangesAsync();

                foreach(var fileKey in fileIds)
                {
                    _logger.LogInformation($"fileKey {fileKey}");
                    var Id = Request.Form[fileKey];
                    _logger.LogInformation($"fileKey Id {Id}");
                    foreach(var returnId in Id.ToString().Split(','))
                    {
                        var supportAttachment = await _context.SupportAttachments
                                                            .Where(sa=>sa.Id == int.Parse(returnId))
                                                            .FirstOrDefaultAsync();
                        if(supportAttachment!=null)
                        {
                            supportAttachment.SupportMessageId = SupportMessage.Id;
                            await _context.SaveChangesAsync();
                        }
                    }
                    
                }
                return new JsonResult(new {success=true,message="Posted Message successfully"});
            }
            
            return new JsonResult(new {success=false,message="Could Not Send Message"});
         }

         public async Task<IActionResult> OnPostReplyMessageAsync()
         {
             var user = await _userManager.GetUserAsync(User);
            if(user!=null)
            {
                var fileIds = Request.Form.Keys.Where(k=>k=="fileId");
                var SupportMessage = new Support();
                SupportMessage.Message = Request.Form["message"];
                SupportMessage.Read = false;
                SupportMessage.SendById = user.Id;
                // SupportMessage.TicketNumber = await GenerateSupportTicket();
                SupportMessage.CreatedAt = DateTime.Now;
                SupportMessage.UpdatedAt = DateTime.Now;

                _context.SupportMessages.Add(SupportMessage);
                await _context.SaveChangesAsync();
                foreach(var fileKey in fileIds)
                {
                    var Id = Request.Form[fileKey];
                    foreach(var returnId in Id.ToString().Split(','))
                    {
                        var supportAttachment = await _context.SupportAttachments
                                                            .Where(sa=>sa.Id == int.Parse(returnId))
                                                            .FirstOrDefaultAsync();
                        if(supportAttachment!=null)
                        {
                            supportAttachment.SupportMessageId = SupportMessage.Id;
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                 var ReplyToId = Request.Form["ReplyToMessage.Id"];
                 _logger.LogInformation($"ReplyToMessage.Id {ReplyToId}");
                var ReplyToMessage = await _context.SupportMessages.FirstOrDefaultAsync(m=>m.Id==int.Parse(ReplyToId));
                if(ReplyToMessage !=null)
                {
                    _logger.LogInformation("appending to Replies");
                    SupportMessage.ReplyToId = int.Parse(ReplyToId);
                    await _context.SaveChangesAsync();
                }
                return new JsonResult(new {success=true,message="Posted Message successfully"});
            }
            
            return new JsonResult(new {success=false,message="Could Not Send Message"});
         }
         public async Task<IActionResult> OnPostSupportFileAsync()
         {
            var files = Request.Form.Files;
            if(files.Count != 0)
            {
               var baseUrl = await _context.DocumentPaths.FirstOrDefaultAsync();
               if(baseUrl == null)
               {
                _context.DocumentPaths.Add(new DocumentPath{
                    BaseUrl="C:\\Uploads"
                });
                await _context.SaveChangesAsync();
                baseUrl = await _context.DocumentPaths.FirstOrDefaultAsync();
               }
               var uploadPath = Path.Combine(baseUrl.BaseUrl,"SupportUploads");
               _logger.LogInformation(baseUrl.BaseUrl);
               if(!Directory.Exists(uploadPath))
               {
                Directory.CreateDirectory(uploadPath);
               }
               var uniqueFilename = $"{Guid.NewGuid()}_{files[0].FileName}";
               var filePath = Path.Combine(uploadPath,uniqueFilename);
               _logger.LogInformation(filePath);
               using(var stream = new FileStream(filePath,FileMode.Create))
               {
                files[0].CopyTo(stream);
               }
               var supportAttachment = new SupportAttachment{
                FileUrl=filePath,
                DisplayName = files[0].FileName
               };
              
              _context.SupportAttachments.Add(supportAttachment);
               _logger.LogInformation(supportAttachment.Id.ToString());

               await _context.SaveChangesAsync();

               return new JsonResult(new {success=true,Message="uploaded suceessfully",attachmentId=supportAttachment.Id,files[0].FileName});
            }
           return new JsonResult(new {success=false,Message="could not upload file"});
         }

         // Delete Support Message
         public async Task<IActionResult> OnPostDeleteAsync()
         {
            var messageId=Request.Form["Message.Id"];
            if(messageId.ToString() != null)
            {
                var SupportMessage = await _context.SupportMessages.FirstOrDefaultAsync(m=>m.Id==int.Parse(messageId));
                if(SupportMessage !=null)
                {
                    _context.SupportMessages.Remove(SupportMessage);
                    await _context.SaveChangesAsync();
                    return new JsonResult(new {success=true,message="Deleted Message Successfilly"});
                }
                
            }
            return BadRequest();
         }

         // Delete Files
         public async Task<IActionResult> OnPostDeleteFileAsync()
         {
            var fileId = Request.Form["fileId"];
            if(fileId.ToString() !=null)
            {
                var supportAttachment = await _context.SupportAttachments.FirstOrDefaultAsync(a=>a.Id==int.Parse(fileId));
                if(supportAttachment !=null)
                {
                    _context.SupportAttachments.Remove(supportAttachment);
                    await _context.SaveChangesAsync();
                    return new JsonResult(new {success=true,message="Successfully deleted file"});
                }

                
            }
            return BadRequest();
         }

         public async Task<IActionResult> OnPostEditSupportMessageAsync()
         {

            var user = await _userManager.GetUserAsync(User);
            if(user!=null)
            {
                var supportMessageId = Request.Form["Message.Id"];
                var fileIds = Request.Form.Keys.Where(k=>k=="fileId");
                var SupportMessage = await _context.SupportMessages.FirstOrDefaultAsync(m=>m.Id==int.Parse(supportMessageId));
                if (SupportMessage == null)
                {
                    return BadRequest();
                }
                SupportMessage.Message = Request.Form["message"];
                SupportMessage.Read = true;
                // SupportMessage.SendById = user.Id;
                // SupportMessage.TicketNumber = await GenerateSupportTicket();
                // SupportMessage.CreatedAt = DateTime.Now;
                SupportMessage.UpdatedAt = DateTime.Now;

                _context.SupportMessages.Update(SupportMessage);
                await _context.SaveChangesAsync();
                foreach(var fileKey in fileIds)
                {
                    var Id = Request.Form[fileKey];
                    foreach(var returnId in Id.ToString().Split(','))
                    {
                        var supportAttachment = await _context.SupportAttachments
                                                            .Where(sa=>sa.Id == int.Parse(returnId))
                                                            .FirstOrDefaultAsync();
                        if(supportAttachment!=null)
                        {
                            supportAttachment.SupportMessageId = SupportMessage.Id;
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                
                return new JsonResult(new {success=true,message="Edit Message successfully"});
            }
            
            return new JsonResult(new {success=false,message="Could Not Edit Message"});
         }

         public async Task<IActionResult> OnGetSupportAttachmentAsync(string Id)
         {
             _logger.LogInformation(Id);
            if(string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }
            else {
                var supportAttachment = await _context.SupportAttachments.FirstOrDefaultAsync(a=>a.Id == int.Parse(Id));
                if(supportAttachment!=null)
                {
                    var fileName = supportAttachment.FileUrl;
                    _logger.LogInformation(fileName);
                    if(!System.IO.File.Exists(fileName))
                    {
                        return NotFound();
                    } else {
                        var provider = new FileExtensionContentTypeProvider();
                        if(!provider.TryGetContentType(fileName,out var contentType))
                        {
                            contentType = "application/octet-stream";
                        }
                        var fileBytes = await System.IO.File.ReadAllBytesAsync(fileName);
                        return File(fileBytes,contentType,fileName);
                    }
                } else {
                    return NotFound();
                }
            }
         }
         public async Task<string> GenerateSupportTicket()
         {
               var CurrentSupportTicketSequence = _context.Database
               .SqlQuery<int>($"SELECT NEXT VALUE FOR SupportTicket AS CurrentValue").AsEnumerable().FirstOrDefault();
               var CurrentDate = DateTime.Now.ToShortDateString();

               var result = "S-TICKET#"+ CurrentSupportTicketSequence.ToString();

                return result.ToUpper();
         }
        
    }
}