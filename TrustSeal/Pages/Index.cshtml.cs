using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace TrustSeal.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly TrustSeal.Areas.Identity.Data.TSAuth _context;

        public IndexModel(ILogger<IndexModel> logger,TrustSeal.Areas.Identity.Data.TSAuth context)
        {
            _logger = logger;
            _context = context;
         }
         public int CurrentBusinessCaseSequence;

        public async void OnGet()
        {
           CurrentBusinessCaseSequence = _context.Database.SqlQuery<int>($"SELECT NEXT VALUE FOR BusinessCaseSequence AS CurrentValue").AsEnumerable().FirstOrDefault();
        }
    }
}
