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
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Security.Claims;
using TrustSeal.Areas.Identity.Data;
using TrustSeal.Models;


namespace TrustSeal.Pages.ApplySeal
{
    [Authorize]
    public class ReviewApplication : PageModel
    {
        private readonly TrustSeal.Areas.Identity.Data.TSAuth _context;
        private readonly UserManager<TSUser> _userManager;
        private readonly ILogger<IndexModel> _logger;
       


        // This is the only constructor
        public ReviewApplication(TrustSeal.Areas.Identity.Data.TSAuth context,
         ILogger<IndexModel> logger,
         UserManager<TSUser> userManager
         )
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }
        [BindProperty]
        public Business Business { get; set; } = default!;

        public IList<QuestionCategory> Criteria { get; set;}
        
        public List<BsAnswer> SubmittedAnswers {get;set;} = new List<BsAnswer>();

        [BindProperty]
        public IFormFile BusinessRegFile {get;set;}

        [BindProperty]
        public IFormFile KRAFile {get;set;}

        [BindProperty]
        public string KRAFileUrl {get;set;}
         [BindProperty]
        public string KRAFileDisplayName {get;set;}

        [BindProperty]
        public string BusinessRegFileUrl {get;set;}

         [BindProperty]
        public string BusinessRegFileDisplayName {get;set;}

        public List<AttachmentConfigs> AttachmentConfigs {get;set;}
        public List<BusinessAttachment> BusinessAttachments {get;set;}

    

        public async Task<IActionResult> OnGetAsync(string? Id)
        {
            _logger.LogInformation("In OnGetAsync");
            Criteria = await _context.QuestionCategories
               .Include(q => q.questions.Where(q=>q.IsActive==true))
               .ToListAsync();
            AttachmentConfigs = await _context.AttachmentConfigs.ToListAsync();

            if (Id != null && Id != string.Empty)
            {
                Business = await _context.Businesses.FirstOrDefaultAsync(b=> b.Id == int.Parse(Id));
                if (Business != null)
                {
                SubmittedAnswers = await _context.BsAnswer.Where(b=> b.BusinessID == Business.Id)
                                    .Include(b=>b.Question)
                                    .ToListAsync();
                BusinessAttachments = await _context.BusinessAttachment.Where(a=>a.BusinessId==Business.Id)
                                                                        .ToListAsync();
                   
                var kraFile = await _context.BusinessAttachment.FirstOrDefaultAsync(a => 
                                        a.ForWhichField == "KRAFile" && a.BusinessId == Business.Id);
                var bsRegFile = await _context.BusinessAttachment.FirstOrDefaultAsync(a => 
                                        a.ForWhichField == "BusinessRegFile" && a.BusinessId == Business.Id);
                if (kraFile != null)
                {
                    KRAFileUrl = kraFile.FileReference;
                    KRAFileDisplayName = kraFile.DisplayName;
                }

                 if (bsRegFile != null)
                {
                    BusinessRegFileUrl = bsRegFile.FileReference;
                    BusinessRegFileDisplayName = bsRegFile.DisplayName;
                }
                _logger.LogInformation($"Business {Business.Id}");
               
                }

                 return Page();
            }
            return Page();
        }

        
        public async Task<IActionResult> OnGetUploadedFileAsync(string Id)
        {
            _logger.LogInformation(Id);
            if(string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }
            else {
                var BusinessAttachment = await _context.BusinessAttachment.FirstOrDefaultAsync(a=>a.Id == int.Parse(Id));
                if(BusinessAttachment!=null)
                {
                    var fileName = BusinessAttachment.FileReference;
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
    }

}
