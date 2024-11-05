using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TrustSeal.Areas.Identity.Data;

namespace TrustSeal.Pages.UserManagement {
    class IndexModel: PageModel {
         private readonly TrustSeal.Areas.Identity.Data.TSAuth _context;
         private readonly UserManager<TSUser> _userManager;
        private readonly ILogger<IndexModel> _logger;

        private readonly RoleManager<IdentityRole> _roleMager;

        public IndexModel(
            TrustSeal.Areas.Identity.Data.TSAuth context,
            ILogger<IndexModel> logger,
            UserManager<TSUser>  userManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _roleMager = roleManager;
        }

        public List<TSUser> Users {get;set;} = new List<TSUser>();
        
        public async Task OnGet()
        {
            Users = await _context.Users
            .ToListAsync();
                                    
        }

        public async Task<IActionResult> OnPostAsync()
        {
            return Page();
        }
    }
}