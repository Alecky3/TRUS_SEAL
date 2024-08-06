using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrustSeal.Areas.Identity.Data;
using TrustSeal.Models;

namespace TrustSeal.Pages.TsQuestionCategories
{
    public class EditModel : PageModel
    {
        private readonly TrustSeal.Areas.Identity.Data.TSAuth _context;

        public EditModel(TrustSeal.Areas.Identity.Data.TSAuth context)
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

            var questioncategory =  await _context.QuestionCategories.FirstOrDefaultAsync(m => m.Id == id);
            if (questioncategory == null)
            {
                return NotFound();
            }
            QuestionCategory = questioncategory;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(QuestionCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionCategoryExists(QuestionCategory.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool QuestionCategoryExists(int id)
        {
            return _context.QuestionCategories.Any(e => e.Id == id);
        }
    }
}
