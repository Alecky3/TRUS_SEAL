using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
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

        public List<AttachmentConfigs> AttachmentConfigs {get;set;}
        public List<BusinessAttachment> BusinessAttachments {get;set;}

    

        public async Task<IActionResult> OnGetAsync(string? Id)
        {
            _logger.LogInformation("In OnGetAsync");
            Criteria = await _context.QuestionCategories
               .Include(q => q.questions).ToListAsync();
            AttachmentConfigs = await _context.AttachmentConfigs.ToListAsync();

            if (Id != null && Id != string.Empty)
            {
                Business = await _context.Businesses.FirstOrDefaultAsync(b=> b.Id == int.Parse(Id));
                if (Business != null)
                {
                SubmittedAnswers = await _context.BsAnswer.Where(b=> b.BusinessID == Business.Id)
                                    .Include(b=>b.Question)
                                    .ToListAsync();
                BusinessAttachments = await _context.BusinessAttachment.Where(a=>a.BusinessId==Business.Id)
                                                                        .ToListAsync();
                   
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
            Handles creation a new business 
        **/
        public async Task<JsonResult> OnPostNewBusinessAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            _logger.LogInformation("In post business");
            var emptyBusiness = new Business();
            emptyBusiness.OwnerId = user.Id;
            _logger.LogInformation("Created an empty business");
            var businessAttachmentIds = Request.Form.Keys.Where(k=>k.EndsWith("attachment"));
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
                    _logger.LogInformation("Try to create Business");
                    _context.Businesses.Add(emptyBusiness);
                    await _context.SaveChangesAsync();
                    Business = emptyBusiness;
                    await _context.Notifications.AddAsync(new Notification {Content=$"Succesfuly Created/Updated Submitted Business Information,Business Name: {Business.LegalName}",
                                                            BusinessId=Business.Id,UserId=user.Id});
                    await _context.SaveChangesAsync();
                    
                    
                    _logger.LogInformation($"Created new business with ID: {Business.Id}");
                     foreach(var attachementId in businessAttachmentIds)
                    {
                        _logger.LogInformation(attachementId);
                        var id = Request.Form[attachementId];
                        _logger.LogInformation(id);
                        if(!string.IsNullOrEmpty(id.ToString()))
                        {
                            _logger.LogInformation(id.ToString());
                            var businessAttachment = await _context.BusinessAttachment.FirstOrDefaultAsync(b=>b.Id==int.Parse(id));
                            _logger.LogInformation(businessAttachment.FileReference);
                            if(businessAttachment!=null)
                            {
                                businessAttachment.CategoryName = "Business Information";
                                businessAttachment.BusinessId = Business.Id;
                                await _context.SaveChangesAsync();
                            }
                        }
                        
                    }

                    var message = new {message="Created Business successfully", business = Business.Id};

                    return new JsonResult(message);
                } else {
                    foreach(var state in ModelState)
                    {
                        var key = state.Key;
                        var errors = state.Value.Errors;

                        foreach(var error in errors)
                        {
                            _logger.LogInformation($"Field {key}, error {error}");
                        }
                    }
                }
                
            // if exist sh
            return new JsonResult(new {message = "could not create business"});
        }

        /** Handle update of business information **/
        public async Task<IActionResult> OnPostUpdateBusinessAsync()
        {
             var user = await _userManager.GetUserAsync(User);
            _logger.LogInformation("In post Update business Information");
            var data = Request.Form;
            var businessId = Request.Form["Business.Id"];
            var businessAttachmentIds = Request.Form.Keys.Where(k=>k.EndsWith("attachment"));
            _logger.LogInformation($"Business.Id {businessId}");

            if (businessId.ToString() != null && businessId.ToString() != string.Empty)
            {
                Business = await _context.Businesses.Where(b => b.Id == int.Parse(businessId)).FirstOrDefaultAsync();
                if(Business != null)
                {
                    if (await TryUpdateModelAsync<Business>(
                    Business,
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

                    _logger.LogInformation("Try to create Business");
                    await _context.SaveChangesAsync();
                    await _context.Notifications.AddAsync(new Notification {Content=$"Succesfuly Updated Submitted Business Information,Business Name: {Business.LegalName}",
                                                            BusinessId=Business.Id,UserId=user.Id});
                    await _context.SaveChangesAsync();
                     foreach(var attachementId in businessAttachmentIds)
                    {
                        _logger.LogInformation(attachementId);
                        var id = Request.Form[attachementId];
                        _logger.LogInformation(id);
                        if(!string.IsNullOrEmpty(id.ToString()))
                        {
                            _logger.LogInformation(id.ToString());
                            var businessAttachment = await _context.BusinessAttachment.FirstOrDefaultAsync(b=>b.Id==int.Parse(id));
                            _logger.LogInformation(businessAttachment.FileReference);
                            if(businessAttachment!=null)
                            {
                                businessAttachment.CategoryName = "Business Information";
                                businessAttachment.BusinessId = Business.Id;
                                await _context.SaveChangesAsync();
                            }
                        }
                        
                    }
                    return new JsonResult(new {message="Update business business successfully",business=Business.Id});
                } else {
                    return new JsonResult(new {error="Could not update business"});
                }
                }

            }
            
            return new JsonResult(new {error="Could not update business"});
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
            && !k.EndsWith("isNextToFinal") && !k.EndsWith("category") && !k.EndsWith("attachment"));
           var bsAnswers = new List<BsAnswer>();
           var GeneratedCaseNumber =  "";
            var businessId = data["Business.Id"];
            var isFinal = data["isNextToFinal"];
            var categoryName = data["category"];
            _logger.LogInformation($"Business.ID = {businessId}");
            var businessAttachmentIds = Request.Form.Keys.Where(k=>k.EndsWith("attachment"));
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
                    foreach(var attachementId in businessAttachmentIds)
                    {
                        _logger.LogInformation(attachementId);
                        var id = Request.Form[attachementId];
                        _logger.LogInformation(id);
                        if(!string.IsNullOrEmpty(id.ToString()))
                        {
                            _logger.LogInformation(id.ToString());
                            var businessAttachment = await _context.BusinessAttachment.FirstOrDefaultAsync(b=>b.Id==int.Parse(id));
                            _logger.LogInformation(businessAttachment.FileReference);
                            if(businessAttachment!=null)
                            {
                                businessAttachment.CategoryName = categoryName;
                                businessAttachment.BusinessId = business.Id;
                                await _context.SaveChangesAsync();
                            }
                        }
                        
                    }
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
                    return new JsonResult(new {error = "Could not save answers",count = bsAnswers.Count()});
             }
        }

        public async Task<IActionResult> OnPostUpdateBusinessAnswerAsync()
        {
            var data = Request.Form;
            var questionKeys = data.Keys.Where(k => ! k.EndsWith("AnswerText") && ! k.EndsWith("Business.Id") 
            && !k.EndsWith("isNextToFinal") && !k.EndsWith("category") && !k.EndsWith("AnswerId") && !k.EndsWith("attachment")) ;
            var bsAnswers = new List<BsAnswer>();
            var GeneratedCaseNumber =  "";
            var businessId = data["Business.Id"];
            var isFinal = data["isNextToFinal"];
            var categoryName = data["category"];
            _logger.LogInformation($"Business.ID = {businessId}");
            var businessAttachmentIds = Request.Form.Keys.Where(k=>k.EndsWith("attachment"));
             if (int.Parse(businessId) != 0 ||  businessId != string.Empty)
                {
                    foreach(var key in questionKeys){
                        var answerKey = Request.Form.Keys.Where(k => k == key+"_AnswerText").First();
                        var bsAnswerKey = Request.Form.Keys.Where(k => k ==key + "_AnswerId").First();
                        var bsAnswer = await _context.Answers.Where(a => a.Id == int.Parse(data[bsAnswerKey])).FirstOrDefaultAsync();
                        // bsAnswer.BusinessID = int.Parse(businessId);
                        // bsAnswer.QuestionID = int.Parse(data[key]);
                        bsAnswer.AnswerText = data[answerKey];
                        bsAnswers.Add(bsAnswer);
                        _logger.LogInformation($"{key}-{answerKey} data{data[key]} - {data[answerKey]}");
                    }
                    _logger.LogInformation($"bsAnswers = {bsAnswers.Count()}");
                     _context.Answers.UpdateRange(bsAnswers);
                    await _context.SaveChangesAsync();
                    var business = await _context.Businesses.FirstOrDefaultAsync(b => b.Id == int.Parse(businessId));
                    foreach(var attachementId in businessAttachmentIds)
                    {
                        _logger.LogInformation(attachementId);
                        var id = Request.Form[attachementId];
                        _logger.LogInformation(id);
                        if(!string.IsNullOrEmpty(id.ToString()))
                        {
                            _logger.LogInformation(id.ToString());
                            var businessAttachment = await _context.BusinessAttachment.FirstOrDefaultAsync(b=>b.Id==int.Parse(id));
                            _logger.LogInformation(businessAttachment.FileReference);
                            if(businessAttachment!=null)
                            {
                                businessAttachment.CategoryName = categoryName;
                                businessAttachment.BusinessId = business.Id;
                                await _context.SaveChangesAsync();
                            }
                        }
                        
                    }
                    return new JsonResult(new {message = "updated Answers successfuly",count = bsAnswers.Count(),CaseNumber = GeneratedCaseNumber });
                }else {
                    return new JsonResult(new {error = "Could not save answers",count = bsAnswers.Count()});
             }

        }

         public async Task<string> GenerateCaseNumber(string BsName)
         {
               var CurrentBusinessCaseSequence =  _context.Database
               .SqlQuery<int>($"SELECT NEXT VALUE FOR BusinessCaseSequence AS CurrentValue")
               .AsEnumerable().FirstOrDefault();
               var CurrentDate = DateTime.Now.ToShortDateString();

               var result = BsName +"/"+ CurrentBusinessCaseSequence.ToString() +"-"+CurrentDate;

                return result.ToUpper();
         }
        
        public async Task<IActionResult> OnGetCaseNumberAsync(string? Id)
        {
            if (Id != null && Id != string.Empty)
            {
                Business = await _context.Businesses.Where(b => b.Id == int.Parse(Id)).FirstOrDefaultAsync();
                if (Business != null)
                {
                    if (Business.Status == "All Answers Submitted")
                    {
                        string CaseNumber;
                        if (Business.CaseNumber == null || Business.CaseNumber == string.Empty)
                        {
                         var firstTwoLetters = Business.LegalName[0].ToString() + Business.LegalName[1].ToString();
                         CaseNumber = await GenerateCaseNumber(firstTwoLetters);
                        } else {
                            CaseNumber = Business.CaseNumber;
                        }
                        return new JsonResult(new {message = "Retrieved CaseNumber successfuly",CaseNumber = CaseNumber });
                    } else {
                         return new JsonResult(new {message = "Fill all remaining Questions"});
                    }
                }
            }

            return new JsonResult(new {error = "Could not Retrieve Business Case Number"});
        }

        public async Task<IActionResult> OnPostBusinessFiles()
        {
            var files = Request.Form.Files;
            if(files.Count != 0)
            {
               var baseUrl = await _context.DocumentPaths.FirstOrDefaultAsync();
               if(baseUrl == null)
               {
                _context.DocumentPaths.Add(new DocumentPath{
                    BaseUrl="C:\\Uploads"
                });
                await _context.SaveChangesAsync();
                baseUrl = await _context.DocumentPaths.FirstOrDefaultAsync();
               }
               _logger.LogInformation(baseUrl.BaseUrl);
               if(!Directory.Exists(baseUrl.BaseUrl))
               {
                Directory.CreateDirectory(baseUrl.BaseUrl);
               }
               var uniqueFilename = $"{Guid.NewGuid()}_{files[0].FileName}";
               var filePath = Path.Combine(baseUrl.BaseUrl,uniqueFilename);
               _logger.LogInformation(filePath);
               using(var stream = new FileStream(filePath,FileMode.Create))
               {
                files[0].CopyTo(stream);
               }
               var businessAttachment = new BusinessAttachment{
                FileReference=filePath,
                DisplayName = files[0].FileName,
                ForWhichField = files[0].Name
               };
               _context.BusinessAttachment.Add(businessAttachment);
               _logger.LogInformation(businessAttachment.Id.ToString());

               await _context.SaveChangesAsync();

               return new JsonResult(new {success=true,Message="uploaded suceessfully",attachmentId=businessAttachment.Id});
            }
           return new JsonResult(new {success=false,Message="could not upload file"});
        }

        public async Task<IActionResult> OnGetUploadedFileAsync(string Id)
        {
            _logger.LogInformation(Id);
            if(string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }
            else {
                var BusinessAttachment = await _context.BusinessAttachment.FirstOrDefaultAsync(a=>a.Id == int.Parse(Id));
                if(BusinessAttachment!=null)
                {
                    var fileName = BusinessAttachment.FileReference;
                    _logger.LogInformation(fileName);
                    if(!System.IO.File.Exists(fileName))
                    {
                        return NotFound();
                    } else {
                        var provider = new FileExtensionContentTypeProvider();
                        if(!provider.TryGetContentType(fileName,out var contentType))
                        {
                            contentType = "application/octet-stream";
                        }
                        var fileBytes = await System.IO.File.ReadAllBytesAsync(fileName);
                        return File(fileBytes,contentType,fileName);
                    }
                } else {
                    return NotFound();
                }
            }
        }
    }

}
