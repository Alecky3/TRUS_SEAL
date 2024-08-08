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
    public class DetailsModel : PageModel
    {
        private readonly TrustSeal.Areas.Identity.Data.TSAuth _context;

        public DetailsModel(TrustSeal.Areas.Identity.Data.TSAuth context)
        {
            _context = context;
        }

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
    }
}
