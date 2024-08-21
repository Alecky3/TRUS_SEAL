using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrustSeal.Areas.Identity.Data;
using TrustSeal.Models;

namespace TrustSeal.Pages.TsBusinessAnswers
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
        ViewData["BusinessID"] = new SelectList(_context.Businesses, "Id", "City");
        ViewData["QuestionID"] = new SelectList(_context.Questions, "Id", "QuestionText");
            return Page();
        }

        [BindProperty]
        public BsAnswer BsAnswer { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.BsAnswer.Add(BsAnswer);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
