using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Security.Claims;
using TrustSeal.Areas.Identity.Data;
using TrustSeal.Models;


namespace TrustSeal.Pages.Notifications
{
    [Authorize]
    public class IndexModel: PageModel {
        private readonly TrustSeal.Areas.Identity.Data.TSAuth _context;
        private readonly UserManager<TSUser> _userManager;
        private readonly ILogger<IndexModel> _logger;
        public IndexModel(TrustSeal.Areas.Identity.Data.TSAuth context,
         ILogger<IndexModel> logger,
         UserManager<TSUser> userManager)
         {
             _context = context;
            _logger = logger;
            _userManager = userManager;
         }

         public List<Notification> Notifications {get;set;} = new List<Notification>();

         public async Task OnGetAsync()
         {
            TSUser user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var role = await _userManager.GetRolesAsync(user);

                if (role.Contains("Admin"))
                {
                    Notifications = await _context.Notifications
                                                    .Include(n=> n.ReplyToNotification)
                                                    .Include(n=> n.CreatedBy)
                                                    .ToListAsync();
                } else {
                    Notifications = await _context.Notifications
                                                    .Where(n => n.UserId == user.Id)
                                                    .ToListAsync();
                }

            }
            
         }
    }
}