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


namespace TrustSeal.Pages.AttachmentConfig
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

         public List<QuestionCategory> QuestionCategories {get;set;}
         public List<AttachmentConfigs> AttachmentConfigs {get;set;}

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if(user!=null)
            {
                var Roles = await _userManager.GetRolesAsync(user);
                if(Roles.Contains("Admin"))
                {
                    QuestionCategories = await _context.QuestionCategories.ToListAsync();
                    AttachmentConfigs = await _context.AttachmentConfigs.ToListAsync();
                    return Page();
                } else {
                    return NotFound();
                }

            } else {
                return NotFound();
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var attachmentConfig = new AttachmentConfigs();
            if(await TryUpdateModelAsync<AttachmentConfigs>(
                attachmentConfig,
                "AttachmentConfig",
                a=>a.Category,
                a=>a.ForWhichField
                
            )){
                attachmentConfig.Required = int.Parse(Request.Form["AttachmentConfig.Required"]) == 0 ? false : true;
                // attachmentConfig.CreatedAt=DateTime.Today;
                // attachmentConfig.UpdatedAt =DateTime.Today;
                _context.AttachmentConfigs.Add(attachmentConfig);
                await _context.SaveChangesAsync();
                return new JsonResult(new{success=true,message="Created Attachment Config Successfully"});
            }
            return new JsonResult(new{success=false,message="Unable to create attachment config"});
        }
        public async Task<IActionResult> OnPostUpdateAttachmentConfigAsync()
        {
            var attachmentConfigId = int.Parse(Request.Form["AttachmentConfig.Id"]);
            var attachmentConfig = await _context.AttachmentConfigs.FirstOrDefaultAsync(a=>a.Id==attachmentConfigId);
            if(attachmentConfig!=null)
            {
                attachmentConfig.Category = Request.Form["AttachmentConfig.Category"];
                attachmentConfig.ForWhichField = Request.Form["AttachmentConfig.ForWhichField"];
                attachmentConfig.Required = int.Parse(Request.Form["AttachmentConfig.Required"]) == 0 ? false : true;
                await _context.SaveChangesAsync();
                return new JsonResult(new {success=true,message="Updated Attachment Config Sucessfully"});
            } else {
                return new JsonResult(new {success=false,message="Could not update Attachment Config"});
            }
            return new JsonResult(new {success=false,message="Could not update Attachment Config"});
        }
    }
}