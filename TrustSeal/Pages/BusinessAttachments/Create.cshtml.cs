using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TrustSeal.Areas.Identity.Data;
using TrustSeal.Models;

namespace TrustSeal.Pages.BusinessAttachments
{
    public class CreateModel : PageModel
    {
        private readonly TrustSeal.Areas.Identity.Data.TSAuth _context;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(TrustSeal.Areas.Identity.Data.TSAuth context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public BsAnswer Answer {get;set;}

        [BindProperty]
        public IList<IFormFile> FormFiles {get;set;}
        public async Task<IActionResult> OnGet(int? BusinessID,int? QuestionID)
        {
            if (BusinessID == null || QuestionID == null)
            {
                return NotFound();
            }
            Answer = await _context.Answers
                                    .Include(a=> a.Question)
                                    .Include(a => a.BusinessAttachment)
                                    .FirstOrDefaultAsync(a=> a.BusinessID == BusinessID && a.QuestionID == QuestionID);
            if (Answer == null)
            {
                return NotFound();
            }

            return Page();
           
        }

       
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int BusinessID,int QuestionID)
        {
            long size = FormFiles.Sum(f=> f.Length);

            //  if (!ModelState.IsValid)
            // {
            //     return Page();
            // }
            _logger.LogInformation($"File sizes = {size}");
           
            if (FormFiles != null && FormFiles.Count > 0)
            {
                

                foreach(var formFile in FormFiles)
                {
                     BusinessAttachment BsAttachement = new BusinessAttachment();
                    var randomFile = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
                    var filePath = Path.Combine("Uploads",randomFile + formFile.FileName);
                    _logger.LogInformation($"filePath={filePath}");

                    using (var stream = new FileStream(filePath,FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                    BsAttachement.FileReference = filePath;
                    BsAttachement.BsAnswerId = BusinessID;
                    BsAttachement.BsAnswerId = QuestionID;

                    _context.BusinessAttachment.Add(BsAttachement);
                     await _context.SaveChangesAsync();
                }

            }
            
           
            
           

            return RedirectToPage("./Index");
        }
    }
}
