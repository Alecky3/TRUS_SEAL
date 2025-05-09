using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TrustSeal.Areas.Identity.Data;
using TrustSeal.Models;

namespace TrustSeal.Pages.TsDashboard
{

    [Authorize]
    public class IndexModel : PageModel
    {
         private readonly TrustSeal.Areas.Identity.Data.TSAuth _context;
        private readonly UserManager<TSUser> _userManager;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
        TrustSeal.Areas.Identity.Data.TSAuth context,
        ILogger<IndexModel> logger,
        UserManager<TSUser> userManager
        )
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }
        public List<TSUser> users {get;set;}
        public List<Seals> seals {get;set;}
        public List<Business> businesses {get;set;}

        public List<Notification> notifications {get;set;}
        public async Task OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if(user !=null)
            {
                var role = await _userManager.GetRolesAsync(user);
                if(role !=null)
                {
                    if(role.Contains("Admin")){
                        users = await _context.Users.ToListAsync();
                        seals = await _context.Seals.ToListAsync();
                        businesses = await _context.Businesses.ToListAsync();
                        notifications = await _context.Notifications.Include(b=>b.CreatedBy).ToListAsync();
                    } else {
                        seals = await _context.Seals.Where(s=>s.Business.OwnerId == user.Id).ToListAsync();
                        businesses = await _context.Businesses.Where(b=>b.OwnerId==user.Id).ToListAsync();
                        notifications = await _context.Notifications.Where(n=>n.UserId==user.Id).ToListAsync();
                    }
                }
            }
        }
    }
}
