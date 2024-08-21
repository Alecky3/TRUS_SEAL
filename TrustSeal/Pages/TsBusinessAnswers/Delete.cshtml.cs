using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TrustSeal.Areas.Identity.Data;
using TrustSeal.Models;

namespace TrustSeal.Pages.TsBusinessAnswers
{
    public class DeleteModel : PageModel
    {
        private readonly TrustSeal.Areas.Identity.Data.TSAuth _context;

        public DeleteModel(TrustSeal.Areas.Identity.Data.TSAuth context)
        {
            _context = context;
        }

        [BindProperty]
        public BsAnswer BsAnswer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bsanswer = await _context.BsAnswer.FirstOrDefaultAsync(m => m.ID == id);

            if (bsanswer == null)
            {
                return NotFound();
            }
            else
            {
                BsAnswer = bsanswer;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bsanswer = await _context.BsAnswer.FindAsync(id);
            if (bsanswer != null)
            {
                BsAnswer = bsanswer;
                _context.BsAnswer.Remove(BsAnswer);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
