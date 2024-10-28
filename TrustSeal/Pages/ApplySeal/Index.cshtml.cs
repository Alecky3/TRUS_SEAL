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
    // [Authorize]
    public class IndexModel : PageModel
    {
        private readonly TrustSeal.Areas.Identity.Data.TSAuth _context;
        // private readonly UserManager<TSUser> _userManager;
        private readonly ILogger<IndexModel> _logger;
       


        // This is the only constructor
        public IndexModel(TrustSeal.Areas.Identity.Data.TSAuth context,
         ILogger<IndexModel> logger
        //  UserManager<TSUser> userManager
         )
        {
            _context = context;
            _logger = logger;
            // _userManager = userManager;
        }

        [BindProperty]
        public Business Business { get; set; } = default!;

        // public List<SelectListItem> Businesses { get; set; }

        public IList<QuestionCategory> Criteria { get; set; } = default!;
        
        public BsAnswer Answer {get;set;} = default!;

    

        public async Task OnGetAsync()
        {
            // var user = await _userManager.GetUserAsync(User);
            // Businesses = _context.Businesses
            // // .Where(b => b.OwnerId == user.Id.ToString())
            // .Select(b => new SelectListItem
            // {
            //     Value = b.Id.ToString(),
            //     Text = b.LegalName
            // })
            // .ToList();

            Criteria = await _context.QuestionCategories
               .Include(q => q.questions).ToListAsync();
        }


        public async Task<JsonResult> OnPostNewBusinessAsync()
        {
            _logger.LogInformation("In post business");
            var emptyBusiness = new Business();
            emptyBusiness.OwnerId = "ca7d7338-1cc4-46ef-a732-ceb48de04572";


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
                    Business = emptyBusiness;
                    _logger.LogInformation($"Created new business with ID: {Business.Id}");

                    var message = new {message="Created Business successfully", business = Business.Id};

                    return new JsonResult(message);
                }
                
            // if exist sh
            return new JsonResult(new {message = "could not create business",business = 0});
        }

        public async Task<JsonResult> OnPostFileUploadsWithFileAsync(IFormFile file)
        {

        var uploadedFilePath = string.Empty;
         if (file != null && file.Length > 0)
         {
            var randomFile = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
             var filePath = Path.Combine("Uploads",randomFile + file.FileName);
             uploadedFilePath = filePath;
             _logger.LogInformation($"filePath={filePath}");

             using (var stream = new FileStream(filePath,FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
         }
          var message = new { Message = "in FileUploads Handler"};

          return new JsonResult(new { message = "file uploaded sucessfully",filename =  uploadedFilePath});
        }

        public async Task<JsonResult> OnPostBusinessAnswerAsync()
        {
            var data = Request.Form;
            var questionKeys = data.Keys.Where(k => ! k.EndsWith("AnswerText") && ! k.EndsWith("Business"));
           var bsAnswers = new List<BsAnswer>();
            var businessId = data["Business"];
            _logger.LogInformation($"Business.ID = {businessId}");
                if (int.Parse(businessId) != 0 ||  businessId != string.Empty)
                {
                    foreach(var key in questionKeys){
                        var answerKey = Request.Form.Keys.Where(k => k == key+"_AnswerText").First();
                        var bsAnswer = new BsAnswer();
                        bsAnswer.BusinessID = int.Parse(businessId);
                        bsAnswer.QuestionID = int.Parse(data[key]);
                        bsAnswer.AnswerText = data[answerKey];
                        bsAnswers.Add(bsAnswer);
                        _logger.LogInformation($"{key}-{answerKey} data{data[key]} - {data[answerKey]}");
                    }
                    _logger.LogInformation($"bsAnswers = {bsAnswers.Count()}");
                    await  _context.Answers.AddRangeAsync(bsAnswers);
                    await _context.SaveChangesAsync();
                    return new JsonResult(new {message = "Saved Answers successfuly",count = bsAnswers.Count()});
                }else {
                    return new JsonResult(new {message = "Could not save answers",count = bsAnswers.Count()});
             }
        }
        
    }

}
