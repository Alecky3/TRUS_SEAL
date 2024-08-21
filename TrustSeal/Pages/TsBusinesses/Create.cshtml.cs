using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrustSeal.Areas.Identity.Data;
using TrustSeal.Models;

namespace TrustSeal.Pages.TsBusinesses
{
    public class CreateModel : PageModel
    {
        private readonly TrustSeal.Areas.Identity.Data.TSAuth _context;

        public CreateModel(TrustSeal.Areas.Identity.Data.TSAuth context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Business Business { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {

            var emptyBusiness = new Business();
            emptyBusiness.OwnerId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            
            Console.WriteLine("-------");
            Console.WriteLine(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (await TryUpdateModelAsync<Business>(
                emptyBusiness,
                "business",   // Prefix for form value.
                b => b.LegalName,
                b => b.RegistrationNumber,
                b => b.TaxIdentificationNumber,
                b => b.IncorporationDate,
                b => b.StreetAddress,
                b => b.City,
                b => b.PostalCode,
                b => b.Country,
                b => b.PhoneNumber,
                b => b.Email,
                b => b.Website,
                b => b.PrimaryContactName,
                b => b.PrimaryContactPhone,
                b => b.PrimaryContactEmail,
                b => b.BusinessType,
                b => b.IndustryCategory,
                b => b.SubmissionDate,
                b => b.OwnerId))
                {
                    // Explicitly set properties not included in the form
                    emptyBusiness.IsVerified = false;

                    _context.Businesses.Add(emptyBusiness);
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Index");
                }

            return Page();
        }
    }
}
