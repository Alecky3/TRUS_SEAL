using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrustSeal.Areas.Identity.Data;
using TrustSeal.Models;

namespace TrustSeal.Pages.TsQuestions
{
    public class CreateModel : PageModel
    {
        private readonly TrustSeal.Areas.Identity.Data.TSAuth _context;

        public CreateModel(TrustSeal.Areas.Identity.Data.TSAuth context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CategoryId"] = new SelectList(_context.QuestionCategories, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Question Question { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Questions.Add(Question);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
