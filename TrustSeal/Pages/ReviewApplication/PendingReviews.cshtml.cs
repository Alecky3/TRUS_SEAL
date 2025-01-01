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


namespace TrustSeal.Pages.ReviewApplication
{
    [Authorize]
    public class PendingReviews: PageModel
    {
        private readonly TrustSeal.Areas.Identity.Data.TSAuth _context;
        private readonly UserManager<TSUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IndexModel> _logger;
        public PendingReviews(TrustSeal.Areas.Identity.Data.TSAuth context,
         ILogger<IndexModel> logger,
         UserManager<TSUser> userManager,RoleManager<IdentityRole> roleManager)
         {
             _context = context;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
         }
         public List<Business> Businesses {get;set;}

    public async Task<IActionResult> OnGetPendingReviewsAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if(user != null)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if(roles.Contains("Admin"))
            {
                Businesses = await _context.Businesses.Where(b=>b.CaseNumber != null && b.CaseNumber != string.Empty && (b.SealReadableId == string.Empty || b.SealReadableId == null) )
                .ToListAsync();

                return Page();
            }
           
        }

        return NotFound();
    }
    }
}