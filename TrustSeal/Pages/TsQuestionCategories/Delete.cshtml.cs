using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TrustSeal.Areas.Identity.Data;
using TrustSeal.Models;

namespace TrustSeal.Pages.TsQuestionCategories
{
    public class DeleteModel : PageModel
    {
        private readonly TrustSeal.Areas.Identity.Data.TSAuth _context;

        public DeleteModel(TrustSeal.Areas.Identity.Data.TSAuth context)
        {
            _context = context;
        }

        [BindProperty]
        public QuestionCategory QuestionCategory { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questioncategory = await _context.QuestionCategories.FirstOrDefaultAsync(m => m.Id == id);

            if (questioncategory == null)
            {
                return NotFound();
            }
            else
            {
                QuestionCategory = questioncategory;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questioncategory = await _context.QuestionCategories.FindAsync(id);
            if (questioncategory != null)
            {
                QuestionCategory = questioncategory;
                _context.QuestionCategories.Remove(QuestionCategory);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
