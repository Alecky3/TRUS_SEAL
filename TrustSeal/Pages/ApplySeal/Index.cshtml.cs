using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TrustSeal.Models;

namespace TrustSeal.Pages.ApplySeal
{
    public class IndexModel : PageModel
    {
        private readonly TrustSeal.Areas.Identity.Data.TSAuth _context;

        // This is the only constructor
        public IndexModel(TrustSeal.Areas.Identity.Data.TSAuth context)
        {
            _context = context;
        }

        public IList<QuestionCategory> Criteria { get; set; } = default!;

        public async Task OnGetAsync()
        {
         
            Criteria = await _context.QuestionCategories
               .Include(q => q.questions).ToListAsync();
        }
    }
}
