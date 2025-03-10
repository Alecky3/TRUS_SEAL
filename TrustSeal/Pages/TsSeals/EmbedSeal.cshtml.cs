using Microsoft.AspNetCore.Cors;
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
   
    public class EmbedSeal: PageModel {
        private readonly TrustSeal.Areas.Identity.Data.TSAuth _context;
                private readonly ILogger<IndexModel> _logger;
        public EmbedSeal(TrustSeal.Areas.Identity.Data.TSAuth context,
         ILogger<IndexModel> logger
         )
         {
             _context = context;
            _logger = logger;
         }
        //  [EnableCors("EmbedSealCorsPolicy")]
        public async Task<IActionResult> OnGetEmbedSealAsync(string SealCode)
        {
            Response.Headers.Append("Access-Control-Allow-Origin", "*");
            Response.Headers.Append("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type");
            if (string.IsNullOrEmpty(SealCode))
            {
                return NotFound();
            } 
         
            var EmbedSeal = await _context.Seals
                                                .Include(s=>s.Business)
                                                .ThenInclude(b=>b.Owner)
                                                .Where(s=>s.SealCode.ToString() == SealCode)
                                                .Select(s => new 
                                                {
                                                    CreatedAt = s.CreatedAt,
                                                    ExpiresAt = s.ExpiresAt,
                                                    BusinessLegalName = s.Business.LegalName,
                                                    SealReadableId = s.Business.SealReadableId,
                                                    SealCode = s.SealCode,
                                                    Seal = new {
                                                        png=$"http://localhost:5027{@Url.Content("~/Badges/badge.png")}",
                                                        svg=$"http://localhost:5027{@Url.Content("~/Badges/badge.svg")}"
                                                        },
                                                    SealPreview = $"http://localhost:5027/SealPreview?SealCode={s.SealCode}"
                                                }).FirstOrDefaultAsync();
            if (EmbedSeal != null)
            {
                return new JsonResult(EmbedSeal);
            }
            return NotFound();
              
        }
    }
}