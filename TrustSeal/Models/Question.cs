using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrustSeal.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string QuestionText { get; set; }

        [Required]

        public bool IsActive { get; set; } = true;

        public bool HasChoices { get; set; } = false;

        public List<string> Choices {get;set;} = new List<string> {"Yes","No"};

        [ForeignKey("Category")]
        [Required]
        public int CategoryId { get; set; }

        public QuestionCategory Category { get; set; }

        public List<Notification> Notifications {get;set;} = new List<Notification>();
    }
}
