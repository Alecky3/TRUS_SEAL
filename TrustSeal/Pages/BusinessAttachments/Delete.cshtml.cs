using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TrustSeal.Areas.Identity.Data;
using TrustSeal.Models;

namespace TrustSeal.Pages.BusinessAttachments
{
    public class DeleteModel : PageModel
    {
        private readonly TrustSeal.Areas.Identity.Data.TSAuth _context;

        public DeleteModel(TrustSeal.Areas.Identity.Data.TSAuth context)
        {
            _context = context;
        }

        [BindProperty]
        public BusinessAttachment BusinessAttachment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessattachment = await _context.BusinessAttachment.FirstOrDefaultAsync(m => m.Id == id);
            

            if (businessattachment == null)
            {
                return NotFound();
            }
            else
            {
                BusinessAttachment = businessattachment;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessattachment = await _context.BusinessAttachment.FindAsync(id);
            if (businessattachment != null)
            {
                var fileName = businessattachment.FileReference;

                BusinessAttachment = businessattachment;
                _context.BusinessAttachment.Remove(BusinessAttachment);
                await _context.SaveChangesAsync();
                if (System.IO.File.Exists(fileName))
                {
                    System.IO.File.Delete(fileName);
                }
               
            }

            return RedirectToPage("./Index");
        }
    }
}
