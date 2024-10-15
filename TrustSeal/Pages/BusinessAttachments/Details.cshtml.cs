using System;
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
    public class DetailsModel : PageModel
    {
        private readonly TrustSeal.Areas.Identity.Data.TSAuth _context;

        public DetailsModel(TrustSeal.Areas.Identity.Data.TSAuth context)
        {
            _context = context;
        }

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
    }
}
