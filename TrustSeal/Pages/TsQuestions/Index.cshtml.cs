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

namespace TrustSeal.Pages.TsQuestions
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly TrustSeal.Areas.Identity.Data.TSAuth _context;
        private readonly UserManager<TSUser> _userManager;
        private readonly ILogger<IndexModel> _logger;

         public IndexModel(TrustSeal.Areas.Identity.Data.TSAuth context,
         ILogger<IndexModel> logger,
         UserManager<TSUser> userManager
         )
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public IList<QuestionCategory> Categories { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Categories = await _context.QuestionCategories
                                        .Include(c=>c.questions)
                                        .ToListAsync();
        }
        public async Task<IActionResult> OnPostAddNewQuestionAsync()
        {
            var question = new Question();
            if(await TryUpdateModelAsync<Question>(
                question,
                "question",
                q=>q.QuestionText,
                q=>q.IsActive,
                q=>q.HasChoices,
                q=>q.CategoryId
            )){
                _context.Questions.Add(question);
                await _context.SaveChangesAsync();
                return new JsonResult(new {seccess=true,message="Successfully created question"});
            }
             return new JsonResult(new {seccess=false,message="Error creating question"});
        }
        public async Task<IActionResult> OnPostUpdateQuestionAsync()
        {
            var questionId = Request.Form["question.Id"];
            if(!string.IsNullOrEmpty(questionId.ToString()))
            {
                var question = await _context.Questions.FirstOrDefaultAsync(q=>q.Id==int.Parse(questionId));
                if(question != null)
                {
                    if(await TryUpdateModelAsync<Question>(
                    question,
                    "question",
                    q=>q.QuestionText,
                    q=>q.IsActive,
                    q=>q.HasChoices,
                    q=>q.CategoryId
                    )){
                        
                        await _context.SaveChangesAsync();
                        return new JsonResult(new {seccess=true,message="Successfully Updated question"});
                    }
                }
                
            }
            
            return new JsonResult(new {seccess=false,message="Error updating question"});
        }

        public async Task<IActionResult> OnPostDeleteQuestionAsync()
        {
            var questionId = Request.Form["question.Id"];
            if(!string.IsNullOrEmpty(questionId.ToString()))
            {
                var question = await _context.Questions.FirstOrDefaultAsync(q=>q.Id==int.Parse(questionId));
                if(question!=null)
                {
                    _context.Questions.Remove(question);
                    await _context.SaveChangesAsync();
                    return new JsonResult(new {seccess=true,message="Deleted Question Successfully"});

                }
            }

            return new JsonResult(new {seccess=false,message="Error updating question"});

        }
        public async Task<IActionResult> OnPostAddNewCategoryAsync()
        {
            var category = new QuestionCategory();
            if(await TryUpdateModelAsync<QuestionCategory>(
                category,
                "category",
                c=>c.Name,
                c=>c.Description,
                c=>c.Order
            )){
                 _context.QuestionCategories.Add(category);
                 await _context.SaveChangesAsync();
                  return new JsonResult(new {seccess=true,message="Successfully Created Question Category"});
            }
             return new JsonResult(new {seccess=true,message="Error creating category"});
        }
        public async Task<IActionResult> OnPostUpdateCategoryAsync()
        {
            var categoryId = Request.Form["category.Id"];
            if(!string.IsNullOrEmpty(categoryId.ToString()))
            {
                var category = await _context.QuestionCategories.Where(c=>c.Id==int.Parse(categoryId)).FirstOrDefaultAsync();
                if(category != null)
                {
                    if(await TryUpdateModelAsync<QuestionCategory>(
                        category,
                        "category",
                        c=>c.Name,
                        c=>c.Description,
                        c=>c.Order
                    )){
                        await _context.SaveChangesAsync();
                        return new JsonResult(new {seccess=true,message="Successfully Updated Question Category"});
                    }
                }
                
            }
            
             return new JsonResult(new {seccess=false,message="Error updating Category"});
        }

        public async Task<IActionResult> OnPostDeleteCategoryAsync()
        {
            var categoryId = Request.Form["category.Id"];
            if(!string.IsNullOrEmpty(categoryId.ToString()))
            {
                var category = await _context.QuestionCategories.FirstOrDefaultAsync(q=>q.Id==int.Parse(categoryId));
                if(category!=null)
                {
                    _context.QuestionCategories.Remove(category);
                    await _context.SaveChangesAsync();
                    return new JsonResult(new {seccess=true,message="Deleted Category Successfully"});

                }
            }

            return new JsonResult(new {seccess=false,message="Error Deleting question"});
        }
        public async Task<IActionResult> OnPostAddQuestionChoicesAsync()
        {
            var questionId = Request.Form["question.Id"];
            _logger.LogInformation($"Question Id {questionId}");
            if(!string.IsNullOrEmpty(questionId.ToString()))
            {
                var question = await _context.Questions.FirstOrDefaultAsync(q=>q.Id==int.Parse(questionId));
                _logger.LogInformation($"Question {question.Id}");
                if(question !=null)
                {
                    var choices=Request.Form["choice"].ToString().Split(',');
                    _logger.LogInformation($"choices {choices.Count()}");
                    if(question.Choices !=null)
                    {
                        question.Choices.AddRange(choices);
                    } else {
                        question.Choices = choices.ToList<string>();
                    }
                    _logger.LogInformation($"choices  2 {choices.Count()}");
                    await _context.SaveChangesAsync();
                   return new JsonResult(new {seccess=true,message="Successfully Added Question Choice(s)"}); 
                }
            }
            return new JsonResult(new {seccess=false,message="Error Adding Question Choice(s)"});
        }
    }
}
