using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.SqlServer.Server;
using TrustSeal.Models;

namespace TrustSeal.Areas.Identity.Data
{
    public class QuestionSeeder
    {

        public static void Initialize(TSAuth context)
        {
            if (context.QuestionCategories.Any())
            {

                return;

            }

            var cat1 = new QuestionCategory
            {
                
                Name = "Web Check",
                Description = "Web Platform Check"

            };
            var cat2 = new QuestionCategory
            {
                 
                Name = "Security Check",
                Description = "Security Of Platform Check"

            }; 
            var cat3 = new QuestionCategory
            {
                 
                Name = "Supply Chain Check",
                Description = "Supply Chain Check",

            };

            var QCategories = new QuestionCategory[]
           {
                cat1,
                cat2,
                cat3
           };

            context.AddRange(QCategories);
            context.SaveChanges();

            var questionsList = new Question[]
{
    new Question
    {

        QuestionText = "Does your website have an imprint section?",
        CategoryId = cat1.Id,
        IsActive = true
    },
    new Question
    {

        QuestionText = "Does the imprint section contain all your relevant business details namely Business name, location, year of registration, business description (products/services)?",
        CategoryId = cat1.Id,
        IsActive = true
    },
    new Question
    {
        QuestionText = "Do you have return policies in place? Please attach.",
        CategoryId = cat1.Id,
        IsActive = true
    },
    new Question
    {

        QuestionText = "Are Prices shown according to the requirements? (Filtered/categorized)",
        CategoryId = cat1.Id,
        IsActive = true
    },
    new Question
    {

        QuestionText = "Do you have a money-back guarantee?",
        CategoryId = cat1.Id,
        IsActive = true
    },
    new Question
    {

        QuestionText = "Do your product photos show the product as sold/out of stock?",
        CategoryId = cat1.Id,
        IsActive = true
    },
    new Question
    {

        QuestionText = "Do you allow reviews of products sold on your website?",
        CategoryId = cat1.Id,
        IsActive = true
    },
    new Question
    {

        QuestionText = "Do you allow public reviews of your business/website?",
        CategoryId = cat1.Id,
        IsActive = true
    },
    new Question
    {

        QuestionText = "Do you have a functional complaints handling and redress system? Please attach or describe.",
        CategoryId = cat1.Id,
        IsActive = true
    },
    new Question
    {

        QuestionText = "Do you have an active SSL certification?",
        CategoryId = cat2.Id,
        IsActive = true
    },
    new Question
    {

        QuestionText = "Please enter your ecommerce website URL",
        CategoryId = cat2.Id,
        IsActive = true
    },
    new Question
    {

        QuestionText = "Do you make use of approved payment providers?",
        CategoryId = cat2.Id,
        IsActive = true
    },
    new Question
    {

        QuestionText = "What payment providers do you use?",
        CategoryId = cat2.Id,
        IsActive = true
    },
    new Question
    {

        QuestionText = "Do you have a Data Security Policy?",
        CategoryId = cat2.Id,
        IsActive = true
    },
    new Question
    {

        QuestionText = "Please attach",
        CategoryId = cat2.Id,
        IsActive = true
    },
    new Question
    {

        QuestionText = "Are you compliant with the Office of the Data Protection Commissioner? If yes, either as a data controller or processor",
        CategoryId = cat2.Id,
        IsActive = true
    },
    new Question
    {

        QuestionText = "Do you have any integrations with third-party logistics providers?",
        CategoryId = cat2.Id,
        IsActive = true
    },
    new Question
    {

        QuestionText = "Do you have a qualification process for new suppliers?",
        CategoryId = cat3.Id,
        IsActive = true
    },
    new Question
    {
        QuestionText = "Do you have a qualification process for new products?",
        CategoryId = cat3.Id,
        IsActive = true
    },
    new Question
    {
        
        QuestionText = "Do you keep stock?",
        CategoryId = cat3.Id,
        IsActive = true
    },
    new Question
    {

        QuestionText = "Do you have your own warehousing facilities?",
        CategoryId = cat3.Id,
        IsActive = true
    },
    new Question
    {

        QuestionText = "Do you use 3rd party warehousing facilities?",
        CategoryId = cat3.Id,
        IsActive = true
    },
    new Question
    {

        QuestionText = "What warehousing companies are you using?",
        CategoryId = cat3.Id,
        IsActive = true
    },
    new Question
    {

        QuestionText = "Do you have your own transportation vehicles?",
        CategoryId = cat3.Id,
        IsActive = true
    },
    new Question
    {

        QuestionText = "Do you make use of 3rd party courier companies?",
        CategoryId = cat3.Id,
        IsActive = true
    },
    new Question
    {

        QuestionText = "What companies are you using?",
        CategoryId = cat3.Id,
        IsActive = true
    },
    new Question
    {

        QuestionText = "Do you send out a notification to confirm order placement, tracking and delivery?",
        CategoryId = cat3.Id,
        IsActive = true
    }
};

            context.Questions.AddRange(questionsList);
            context.SaveChanges();






        }
    }
}
