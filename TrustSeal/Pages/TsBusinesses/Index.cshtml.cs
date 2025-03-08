using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TrustSeal.Areas.Identity.Data;
using TrustSeal.Models;

namespace TrustSeal.Pages.TsBusinesses
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly TrustSeal.Areas.Identity.Data.TSAuth _context;
         private readonly UserManager<TSUser> _userManager;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(TrustSeal.Areas.Identity.Data.TSAuth context,
        UserManager<TSUser> userManager,
        ILogger<IndexModel> logger
        )
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }


        public IList<Business> Business { get;set; } = default!;
    

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
                if(user !=null)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles.Contains("Admin"))
                    {
                        Business = await _context.Businesses
                                                    .ToListAsync();

                        return Page();

                    } else {
                        Business = await _context.Businesses.Where(b=> b.OwnerId == user.Id)
                        .ToListAsync();
                       return Page();
                    }
                }
              
            return NotFound();
        }
        public async Task<IActionResult> OnGetBusinessesAsync()
        {
                var user = await _userManager.GetUserAsync(User);
                if(user !=null)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles.Contains("Admin"))
                    {
                        Business = await _context.Businesses
                                                    .Where(b=> b.CaseNumber != null && b.CaseNumber != "")
                                                    .ToListAsync();
                        var result = Business.Select(b=> new {
                            b.LegalName,b.RegistrationNumber,b.TaxIdentificationNumber,
                            b.Country,b.PhoneNumber,b.Email,b.IsVerified,b.SubmissionDate,b.CaseNumber
                        }).ToList();

                        return new JsonResult(result);

                    } else {
                        Business = await _context.Businesses
                                                    .Where(b=> b.CaseNumber != null && b.CaseNumber != "" && b.OwnerId == user.Id)
                                                    .ToListAsync();
                         var result = Business.Select(b=> new {
                            b.LegalName,b.RegistrationNumber,b.TaxIdentificationNumber,
                            b.Country,b.PhoneNumber,b.Email,b.IsVerified,b.SubmissionDate,b.CaseNumber
                        }).ToList();

                        return new JsonResult(result);
                    }
                }
              
                
                return new JsonResult(new {});
           
            
        }
    }
}
