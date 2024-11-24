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


namespace TrustSeal.Pages.TsSeals
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
        public class AllSeals {
            public DateTime CreatedAt {get;set;}
            public DateTime ExpiresAt {get;set;}

            public string BusinessLegalName {get;set;}

            public string SealReadableId {get;set;}

            public Guid SealCode {get;set;}
        }
        public IList<AllSeals> Seals {get;set;} = new List<AllSeals>();

        public async Task OnGet()
        {
            
        }
        public async Task<IActionResult> OnGetAllSealsAsync()
        {
            TSUser user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var role = await _userManager.GetRolesAsync(user);
                if (role.Contains("Admin"))
                {
                    Seals = await _context.Seals
                                                .Include(s=>s.Business)
                                                .ThenInclude(b=>b.Owner)
                                                .Select(s => new AllSeals
                                                {
                                                    CreatedAt = s.CreatedAt,
                                                    ExpiresAt = s.ExpiresAt,
                                                    BusinessLegalName = s.Business.LegalName,
                                                    SealReadableId = s.Business.SealReadableId,
                                                    SealCode = s.SealCode
                                                }
                                                ).ToListAsync();
                } else {
                    Seals = await _context.Seals
                                                .Include(s=>s.Business)
                                                .ThenInclude(b=>b.Owner)
                                                .Where(s=>s.Business.OwnerId == user.Id)
                                                .Select(s => new AllSeals
                                                {
                                                    CreatedAt = s.CreatedAt,
                                                    ExpiresAt = s.ExpiresAt,
                                                    BusinessLegalName = s.Business.LegalName,
                                                    SealReadableId = s.Business.SealReadableId,
                                                    SealCode = s.SealCode
                                                })
                                                .ToListAsync();
                }
                 return new JsonResult(Seals);
            }
           return new JsonResult(Seals);
        }
    }
}