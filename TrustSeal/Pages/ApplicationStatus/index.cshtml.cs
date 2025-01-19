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


namespace TrustSeal.Pages.ApplicationStatus
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
         public List<ApplicationTracking> ApplicationStatus {get;set;} = new List<ApplicationTracking>();

         public async Task OnGetAsync()
         {
            ApplicationStatus = await _context.ApplicationTrackings
                                                .Where(s=>s.IsMain==true).ToListAsync();
         }

         public async Task<IActionResult> OnPostCreateNewAsync()
         {
            return BadRequest();
         }

         public async Task<IActionResult> OnPostUpdateStatusAsync()
         {
            var StatusId = Request.Form["applicationStatus.Id"];
           
            if(StatusId.ToString() != null)
            {
                var applicationTracking = await _context.ApplicationTrackings.FirstOrDefaultAsync(s=>s.Id==int.Parse(StatusId));
                if(applicationTracking !=null)
                {
                    applicationTracking.Required = Request.Form["applicationStatus.Required"].ToString() == "True" ? true:false;
                    applicationTracking.Status = Request.Form["applicationStatus.Status"];
                    applicationTracking.Order = int.Parse(Request.Form["applicationStatus.Order"]);
                    await _context.SaveChangesAsync();

                    return new JsonResult(new {success=true,message="successfully update status Config"});
                } 
            }
            return BadRequest();
         }

         public async Task<IActionResult> OnPostChangeOrderAsync()
         {
            return BadRequest();
         }
         public async Task<IActionResult> OnPostChangeStatusAsync()
         {
            return BadRequest();
         }

         public async Task<IActionResult> OnPostAddNewStatusAsync()
         {
            _logger.LogInformation("In create new application status");
            var applicationTrackingStatus = new ApplicationTracking();
            _logger.LogInformation("Creating new empty Application Tracking Status");
            if( await TryUpdateModelAsync<ApplicationTracking>(
                applicationTrackingStatus,
                "applicationStatus",
                s=> s.IsMain,
                s=>s.Status,
                s=>s.Required
            )){
                var lastStatus= await _context.ApplicationTrackings.OrderByDescending(a=>a.Order)
                                .FirstOrDefaultAsync();
                _logger.LogInformation("Creating new Application Tracking Status");
                int order = 1;
                if(lastStatus != null)
                {
                    order=lastStatus.Order + 1;
                }
                applicationTrackingStatus.Order = order;
                applicationTrackingStatus.CreatedAt = DateTime.Now;
                applicationTrackingStatus.UpdatedAt = DateTime.Now;
                
                _context.ApplicationTrackings.Add(applicationTrackingStatus);

                await _context.SaveChangesAsync();
                return new JsonResult(new {success=true,message="Created Verification Status successfully"});
            }
            return BadRequest();
         }
    }
}