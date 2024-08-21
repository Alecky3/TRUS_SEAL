using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrustSeal.Models
{
    public class BsAnswer
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int BusinessID { get; set; }

        [ForeignKey("BusinessID")]
        public Business Business { get; set; }

        [Required]
        public int QuestionID { get; set; }

        [ForeignKey("QuestionID")]
        public Question Question { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string AnswerText { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string? FileReference { get; set; }

        [Required]
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string Status { get; set; } = "Submitted";
    }
}