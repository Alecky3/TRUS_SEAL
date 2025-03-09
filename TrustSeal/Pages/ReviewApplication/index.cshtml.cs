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


namespace TrustSeal.Pages.ReviewApplication
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
    public Business Business {get;set;}
    public List<BsAnswer> BsAnswer {get;set;}

    public List<QuestionCategory> Categories {get;set;}

    public List<ApplicationTracking> BusinessApplicationTrackings {get;set;}

    public List<BusinessAttachment> BusinessAttachments {get;set;}

    public List<Business> Businesses {get;set;}
    public async Task<IActionResult> OnGetAsync(string CaseNumber)
    {
        if (CaseNumber == null)
        {
            return NotFound();
        }
        else {
            Business = await _context.Businesses.FirstOrDefaultAsync(b => b.CaseNumber == CaseNumber);
            if (Business != null)
            {
                BsAnswer = await _context.Answers
                                    .Include(a => a.Question)
                                    .ThenInclude(q => q.Category)
                                    .Where(a => a.BusinessID == Business.Id)
                                                .ToListAsync();
                Categories = await _context.QuestionCategories.ToListAsync();    
                BusinessApplicationTrackings = await _context.ApplicationTrackings
                                                    .Where(t => t.BusinessId == Business.Id)
                                                    .ToListAsync();  
                BusinessAttachments = await _context.BusinessAttachment.Where(a => a.BusinessId == Business.Id)
                                                                        .ToListAsync();   
                return Page();                    
            }else {
                return NotFound();
            }
          
        }

        return NotFound();
    }

  

    public async void OnPost()
    {

    }
    public async Task<IActionResult> OnPostBusinessApplicationStatusAsync()
    {
        var keys = Request.Form;
        var trackingId = keys["trackingId"];
        var trackingComment = keys[keys.Keys.Where(k=> k.StartsWith("trackingComment")).FirstOrDefault()];
        var trackingRequired = keys[keys.Keys.Where(k=> k.StartsWith("trackingRequired")).FirstOrDefault()];
        var trackingVerified = keys[keys.Keys.Where(k=> k.StartsWith("trackingVerified")).FirstOrDefault()];
        var isFinalStep = keys[keys.Keys.Where(k=> k.StartsWith("isFinalStep")).FirstOrDefault()];
        string SealReadableId = string.Empty;
        string SealCode = string.Empty;

        var applicationTrackingStatus = await _context.ApplicationTrackings.FirstOrDefaultAsync(a => a.Id == int.Parse(trackingId));
        Business business;
        if (applicationTrackingStatus != null)
        {
            business = await _context.Businesses.Where(b=> b.Id == applicationTrackingStatus.BusinessId)
                                                .FirstOrDefaultAsync();

            if (isFinalStep == "true" && business != null)
            {
                    applicationTrackingStatus.Comments = trackingComment;
                    applicationTrackingStatus.Required = trackingRequired == "true" ? true : false;
                    applicationTrackingStatus.StepVerified = trackingVerified == "true" ? true : false;
                    await _context.SaveChangesAsync();

                    var statuses = await _context.ApplicationTrackings
                                                    .Where(t => t.BusinessId == business.Id && t.Required == true)
                                                    .Select(b => b.StepVerified)
                                                    .ToListAsync();
                    bool notVerified = statuses.Contains(false);
                    if (notVerified)
                    {
                        return new JsonResult(new {error = "Could not verify Seal Generation, Looks like some required steps have not been verified"});
                    }
                    business.IsVerified=true;
                    await _context.SaveChangesAsync();
                    string bsName = business.LegalName[0].ToString() + business.LegalName[1].ToString();
                    if (business.SealReadableId != string.Empty)
                    {
                        var sealC=await _context.Seals.FirstOrDefaultAsync(s=>s.Id==business.SealId);
                        return new JsonResult(new {info = "Seal already Generated, you can renew it if expired",sealCode=sealC.SealCode.ToString()});
                    }
                    var sealId = await GenerateSealReadableId(bsName);
                
                    business.SealReadableId = sealId;
                    business.Seal = new Seals();
                    SealReadableId = sealId;
                    var seal = await _context.Seals.FirstOrDefaultAsync(s=>s.Id== business.SealId);
                    SealCode= seal.SealCode.ToString();

                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Generated Seal Id Successfully-{sealId}");
            } else {
            applicationTrackingStatus.Comments = trackingComment;
            applicationTrackingStatus.Required = trackingRequired == "true" ? true : false;
            applicationTrackingStatus.StepVerified = trackingVerified == "true" ? true : false;
            await _context.SaveChangesAsync();
            }
          return new JsonResult(new {message = "Update application Status Successfully",sealId=SealReadableId,sealCode=SealCode});
        }
       
        _logger.LogInformation($"Tracking Id {trackingId} - {isFinalStep} - {trackingRequired} - {trackingVerified}");
        
        return new JsonResult(new {error = "Could Not Create Seal"});
    }

    /** This is the method for generating Seal Number **/
    public async Task<string> GenerateSealReadableId(string BsName)
         {
               var CurrentBusinessCaseSequence = _context.Database
               .SqlQuery<int>($"SELECT NEXT VALUE FOR SealReadableId AS CurrentValue").AsEnumerable().FirstOrDefault();
               var CurrentDate = DateTime.Now.ToShortDateString();

               var result = "TS/" + BsName +"/"+ CurrentBusinessCaseSequence.ToString() +"-"+CurrentDate;

                return result.ToUpper();
         }
        
    }
}