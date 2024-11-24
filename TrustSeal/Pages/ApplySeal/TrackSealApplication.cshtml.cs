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
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Security.Claims;
using TrustSeal.Areas.Identity.Data;
using TrustSeal.Models;


namespace TrustSeal.Pages.ApplySeal
{
    [Authorize]
    public class TrackSealApplication : PageModel
    {
        private readonly TrustSeal.Areas.Identity.Data.TSAuth _context;
        private readonly UserManager<TSUser> _userManager;
        private readonly ILogger<IndexModel> _logger;
       


        // This is the only constructor
        public TrackSealApplication(TrustSeal.Areas.Identity.Data.TSAuth context,
         ILogger<IndexModel> logger,
         UserManager<TSUser> userManager
         )
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public Business Business {get;set;}

        public List<ApplicationTracking> ApplicationTrackings {get;set;}

        public async Task<IActionResult> OnGetAsync(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            Business = await _context.Businesses.FirstOrDefaultAsync(b => b.Id == Id);
            ApplicationTrackings = await _context.ApplicationTrackings
                                            .Where(t => t.BusinessId == Business.Id)
                                            .ToListAsync();

            if (Business == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}