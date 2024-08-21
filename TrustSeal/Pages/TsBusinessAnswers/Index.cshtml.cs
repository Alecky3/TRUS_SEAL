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
    public class IndexModel : PageModel
    {
        private readonly TrustSeal.Areas.Identity.Data.TSAuth _context;

        public IndexModel(TrustSeal.Areas.Identity.Data.TSAuth context)
        {
            _context = context;
        }

        public IList<BsAnswer> BsAnswer { get;set; } = default!;

        public async Task OnGetAsync()
        {
            BsAnswer = await _context.BsAnswer
                .Include(b => b.Business)
                .Include(b => b.Question).ToListAsync();
        }
    }
}
