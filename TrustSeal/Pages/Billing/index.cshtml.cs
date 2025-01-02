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


namespace TrustSeal.Pages.Billing
{
    [Authorize]
    public class IndexModel: PageModel
    {
        private readonly TrustSeal.Areas.Identity.Data.TSAuth _context;
        private readonly UserManager<TSUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IndexModel> _logger;
        public IndexModel(TrustSeal.Areas.Identity.Data.TSAuth context,
         ILogger<IndexModel> logger,
         UserManager<TSUser> userManager,RoleManager<IdentityRole> roleManager)
         {
             _context = context;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
         }
         public List<UserBillings> UserBillings {get;set;}

         public async Task<IActionResult> OnGetAsync()
         {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var Roles = await _userManager.GetRolesAsync(user);
                if (Roles.Contains("Admin"))
                {
                    UserBillings = await _context.Payments
                                                 .Include(p=>p.PaidBy)
                                                 .ToListAsync();
                    return Page();
                } else {
                    UserBillings = await _context.Payments
                                                    .Include(p=>p.PaidBy)
                                                    .Where(p=>p.PaidBy.Id == user.Id)
                                                    .ToListAsync();
                    return Page();
                }
            }

            return NotFound();
         }
         public async Task<IActionResult> OnPostAsync()
         {
            _logger.LogInformation("In post Billing info");
             var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var Roles = await _userManager.GetRolesAsync(user);
                if (Roles.Contains("Admin"))
                { 
                    _logger.LogInformation(Request.Form["Billing.Email"]);
                    var paidBy = await _context.Users.FirstOrDefaultAsync(u=>u.Email == Request.Form["Billing.Email"].ToString());
                    if (paidBy != null)
                    {
                        var userBilling = new UserBillings();
                        userBilling.CreatedById = user.Id;
                        userBilling.PaidById = paidBy.Id;
                    
                        if( await TryUpdateModelAsync<UserBillings>(
                            userBilling,
                            "billing",
                            b=> b.Amount,
                            b=> b.PaymentCode,
                            b=> b.Reason,
                            b=>b.PaymentMethod
                        ))
                        {
                            _logger.LogInformation(Request.Form["Business.PaymentMethod"]);
                            // userBilling.PaymentMethod = "Bank";
                            await _context.Payments.AddAsync(userBilling);
                            await _context.SaveChangesAsync();

                            _logger.LogInformation("Created User Billing information successfully");
                            return new OkResult();
                        }
                    }
                }
            }

            return new JsonResult(new {error="Could not create new Billing information"});
         }

    }
}