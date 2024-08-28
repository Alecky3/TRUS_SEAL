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
    public class DetailsModel : PageModel
    {
        private readonly TrustSeal.Areas.Identity.Data.TSAuth _context;

        public DetailsModel(TrustSeal.Areas.Identity.Data.TSAuth context)
        {
            _context = context;
        }

        public BsAnswer BsAnswer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bsanswer = await _context.Answers.FirstOrDefaultAsync(m => m.BusinessID == id);
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
    }
}
