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

         public async Task<IActionResult> OnGetAsync()
         {
            var user = await _userManager.GetUserAsync(User);
            if(user!=null)
            {
                var Roles = await _userManager.GetRolesAsync(user);
                if(Roles.Contains("Admin"))
                {
                    SupportMessages = await _context.SupportMessages
                                                    .Include(m=>m.SendBy)
                                                    .Include(m=>m.ReplyTo)
                                                    .Include(m=>m.SupportAttachments)
                                                    .ToListAsync();
                    return Page();
                } else {
                    SupportMessages = await _context.SupportMessages
                                                    .Include(m=>m.SendBy)
                                                    .Include(m=>m.ReplyTo)
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
                    var Id = Request.Form[fileKey];
                    var supportAttachment = await _context.SupportAttachments
                                                            .Where(sa=>sa.Id == int.Parse(Id))
                                                            .FirstOrDefaultAsync();
                    if(supportAttachment!=null)
                    {
                        supportAttachment.SupportMessageId = supportAttachment.Id;
                        await _context.SaveChangesAsync();
                    }
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
                Directory.CreateDirectory(baseUrl.BaseUrl);
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

               return new JsonResult(new {success=true,Message="uploaded suceessfully",attachmentId=supportAttachment.Id});
            }
           return new JsonResult(new {success=false,Message="could not upload file"});
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

               var result = "S-TICKET#"+ CurrentSupportTicketSequence.ToString() +"-";

                return result.ToUpper();
         }
        
    }
}