using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Security.Claims;
using TrustSeal.Models;

namespace TrustSeal.Pages.ApplySeal
{
    public class IndexModel : PageModel
    {
        private readonly TrustSeal.Areas.Identity.Data.TSAuth _context;
        private readonly ILogger<IndexModel> _logger;


        // This is the only constructor
        public IndexModel(TrustSeal.Areas.Identity.Data.TSAuth context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public Business Business { get; set; } = default!;

        public List<SelectListItem> Businesses { get; set; }

        public IList<QuestionCategory> Criteria { get; set; } = default!;

    

        public async Task OnGetAsync()
        {
            Businesses = _context.Businesses
            .Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.LegalName
            })
            .ToList();

            Criteria = await _context.QuestionCategories
               .Include(q => q.questions).ToListAsync();
        }


        public async Task<IActionResult> OnPostAsync()
        {

            int bsID;
            if (!int.TryParse(Request.Form["SelectedBusiness"], out bsID) || bsID == 0)
            {
                var emptyBusiness = new Business();
                emptyBusiness.OwnerId = User.FindFirst(ClaimTypes.NameIdentifier).Value;


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

                }
                
                bsID = emptyBusiness.Id; // Assuming Id is the primary key of Business
                _logger.LogInformation("Created new business with ID: {BusinessID}", bsID);
            }

            var businessAnswers = new List<BsAnswer>();


            foreach (var key in Request.Form.Keys.Where(k => int.TryParse(k, out _)))
            {
                if (int.TryParse(key, out int questionId))
                {
                    var value = Request.Form[key].ToString();
                    var answer = new BsAnswer
                    {
                        BusinessID = bsID,
                        QuestionID = questionId,
                        AnswerText = value
                    };
                    businessAnswers.Add(answer);

                    _logger.LogInformation("BsAnswer: BusinessID={BusinessID}, QuestionID={QuestionID}, AnswerText={AnswerText}",
                        answer.BusinessID,
                        answer.QuestionID,
                        answer.AnswerText);
                }
            }

            await _context.Answers.AddRangeAsync(businessAnswers);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
