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
    public class IndexModel : PageModel
    {
        private readonly TrustSeal.Areas.Identity.Data.TSAuth _context;
        private readonly UserManager<TSUser> _userManager;
        private readonly ILogger<IndexModel> _logger;
       


        // This is the only constructor
        public IndexModel(TrustSeal.Areas.Identity.Data.TSAuth context,
         ILogger<IndexModel> logger,
         UserManager<TSUser> userManager
         )
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }
        [BindProperty]
        public Business Business { get; set; } = default!;

        public IList<QuestionCategory> Criteria { get; set;}
        
        public List<BsAnswer> SubmittedAnswers {get;set;} = new List<BsAnswer>();

        [BindProperty]
        public IFormFile BusinessRegFile {get;set;}

        [BindProperty]
        public IFormFile KRAFile {get;set;}

        [BindProperty]
        public string KRAFileUrl {get;set;}
         [BindProperty]
        public string KRAFileDisplayName {get;set;}

        [BindProperty]
        public string BusinessRegFileUrl {get;set;}

         [BindProperty]
        public string BusinessRegFileDisplayName {get;set;}

    

        public async Task<IActionResult> OnGetAsync(string? Id)
        {
            _logger.LogInformation("In OnGetAsync");
            Criteria = await _context.QuestionCategories
               .Include(q => q.questions).ToListAsync();
            if (Id != null && Id != string.Empty)
            {
                Business = await _context.Businesses.FirstOrDefaultAsync(b=> b.Id == int.Parse(Id));
                if (Business != null)
                {
                   
                var kraFile = await _context.BusinessAttachment.FirstOrDefaultAsync(a => 
                                        a.ForWhichField == "KRAFile" && a.BusinessId == Business.Id);
                var bsRegFile = await _context.BusinessAttachment.FirstOrDefaultAsync(a => 
                                        a.ForWhichField == "BusinessRegFile" && a.BusinessId == Business.Id);
                if (kraFile != null)
                {
                    KRAFileUrl = kraFile.FileReference;
                    KRAFileDisplayName = kraFile.DisplayName;
                }

                 if (bsRegFile != null)
                {
                    BusinessRegFileUrl = bsRegFile.FileReference;
                    BusinessRegFileDisplayName = bsRegFile.DisplayName;
                }
                _logger.LogInformation($"Business {Business.Id}");
               
                }

                 return Page();
            }
            return Page();
        }

        /** 
            Handles creation / update of a new business 
        **/
        public async Task<JsonResult> OnPostNewBusinessAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            _logger.LogInformation("In post business");
            var emptyBusiness = new Business();
            emptyBusiness.OwnerId = user.Id;


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
                    emptyBusiness.Status = "Business Information Submitted";

                    _context.Businesses.Add(emptyBusiness);
                    await _context.SaveChangesAsync();
                    Business = emptyBusiness;
                    await _context.Notifications.AddAsync(new Notification {Content=$"Succesfuly Created/Updated Submitted Business Information,Business Name: {Business.LegalName}",
                                                            BusinessId=Business.Id,UserId=user.Id});
                    await _context.SaveChangesAsync();
                    
                    
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
        /**
             This handler handles file uploads for KRA pin certificate
        **/
        public async Task<JsonResult> OnPostKRAFileAsync()
        {
        var user = await _userManager.GetUserAsync(User);
        var uploadedFilePath = string.Empty;
        var businessId = Request.Form["Business.Id"];
         if (KRAFile != null && KRAFile.Length > 0)
         {
            var randomFile = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
             var filePath = Path.Combine("wwwroot\\Uploads",randomFile + KRAFile.FileName);
             uploadedFilePath = filePath;
             _logger.LogInformation($"filePath={filePath}");

             using (var stream = new FileStream(filePath,FileMode.Create))
                    {
                        await KRAFile.CopyToAsync(stream);
                    }
            var bsAttachment = await _context.BusinessAttachment.AddAsync(new BusinessAttachment {FileReference = filePath,
                                                                    DisplayName=KRAFile.FileName,
                                                                    BusinessId=int.Parse(businessId),
                                                                    ForWhichField = KRAFile.Name});
            await _context.Notifications.AddAsync(new Notification {
                                                        Content ="Saved Kra attachement successfully",
                                                        BusinessId=int.Parse(businessId),
                                                        UserId = user.Id
                                                         });
           await  _context.SaveChangesAsync();
         }
          var message = new { Message = "in FileUploads Handler"};

          return new JsonResult(new { message = "file uploaded sucessfully",filename =  uploadedFilePath,
                                        fileUrl = uploadedFilePath, fileDisplayName=KRAFile.FileName});
        }

        /**
             This handler handles file uploads for Business Registration certificate
        **/
        public async Task<JsonResult> OnPostBusinessRegFileAsync()
        {
        _logger.LogInformation("In post business reg form");
        var user = await _userManager.GetUserAsync(User);
        var uploadedFilePath = string.Empty;
        var businessId = Request.Form["Business.Id"];
         if (BusinessRegFile != null && BusinessRegFile.Length > 0)
         {
            var randomFile = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
             var filePath = Path.Combine("wwwroot\\Uploads",randomFile + BusinessRegFile.FileName);
             uploadedFilePath = filePath;
             _logger.LogInformation($"filePath={filePath}");

             using (var stream = new FileStream(filePath,FileMode.Create))
                    {
                        await BusinessRegFile.CopyToAsync(stream);
                    }
            await _context.BusinessAttachment.AddAsync(new BusinessAttachment {FileReference = filePath,
                                                                    DisplayName=BusinessRegFile.FileName,
                                                                    BusinessId=int.Parse(businessId),
                                                                    ForWhichField = BusinessRegFile.Name});
             await _context.Notifications.AddAsync(new Notification {
                                                        Content ="Saved business registration attachement successfully",
                                                        BusinessId=int.Parse(businessId),
                                                        UserId = user.Id
                                                         });
           await  _context.SaveChangesAsync();
         }
          var message = new { Message = "in FileUploads Handler"};

          return new JsonResult(new { message = "file uploaded sucessfully",filename =  uploadedFilePath,
                                        fileUrl = uploadedFilePath, fileDisplayName=BusinessRegFile.FileName});
        }

        /** 
            This handles creation / update of a Criteria catalog answer for a given 
            Business
        **/
        public async Task<JsonResult> OnPostBusinessAnswerAsync()
        {
            var data = Request.Form;
            var questionKeys = data.Keys.Where(k => ! k.EndsWith("AnswerText") && ! k.EndsWith("Business.Id") 
            && !k.EndsWith("isNextToFinal") && !k.EndsWith("category"));
           var bsAnswers = new List<BsAnswer>();
           var GeneratedCaseNumber =  "";
            var businessId = data["Business.Id"];
            var isFinal = data["isNextToFinal"];
            var categoryName = data["category"];
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
                    // update business status to "[category name] Answers Submitted"
                    var business = await _context.Businesses.FirstOrDefaultAsync(b => b.Id == int.Parse(businessId));
                    if (business != null && isFinal != "true")
                    {
                        string status = categoryName.ToString() + "Answers Submitted";
                        _logger.LogInformation($"Current business Status ${status}");
                        business.Status = status;
                        await _context.SaveChangesAsync();
                    }
                    else if (isFinal.ToString() == "true")
                    {
                        _logger.LogInformation("This is the last step");
                        business.Status = "All Answers Submitted";
                        var firstTwoLetters = business.LegalName[0].ToString() + business.LegalName[1].ToString();
                        var CaseNumber = await GenerateCaseNumber(firstTwoLetters);

                        if (CaseNumber != null)
                        {
                            business.CaseNumber = CaseNumber.ToString();
                            await _context.SaveChangesAsync();
                            GeneratedCaseNumber = CaseNumber;
                            var bsTracking = await _context.ApplicationTrackings
                                                    .Where(t => t.IsMain == true)
                                                    .ToListAsync();
                            _logger.LogInformation($"bs Tracking count {bsTracking.Count()} {business.Id}");
                            foreach(var status in bsTracking)
                            {
                                // _logger.LogInformation("in For each");
                                var appTracking = new ApplicationTracking
                                {
                                Status = status.Status,
                                Comments = string.Empty,
                                BusinessId = business.Id,
                                Required = true,
                                StepVerified = false,
                                NeedsAttention = false,
                                Order = status.Order,
                                IsMain = false,
                                CreatedAt = DateTime.Now,
                                UpdatedAt = DateTime.Now
                                };
                                await _context.ApplicationTrackings.AddAsync(appTracking);
                                await _context.SaveChangesAsync();
                                // _logger.LogInformation("End in For each");
                            }
                            await _context.SaveChangesAsync();
                        }
                    }
                    return new JsonResult(new {message = "Saved Answers successfuly",count = bsAnswers.Count(),CaseNumber = GeneratedCaseNumber });
                }else {
                    return new JsonResult(new {message = "Could not save answers",count = bsAnswers.Count()});
             }
        }

         public async Task<string> GenerateCaseNumber(string BsName)
         {
               var CurrentBusinessCaseSequence = _context.Database
               .SqlQuery<int>($"SELECT NEXT VALUE FOR BusinessCaseSequence AS CurrentValue")
               .AsEnumerable().FirstOrDefault();
               var CurrentDate = DateTime.Now.ToShortDateString();

               var result = BsName +"/"+ CurrentBusinessCaseSequence.ToString() +"-"+CurrentDate;

                return result.ToUpper();
         }
        
        
    }

}
