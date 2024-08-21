using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrustSeal.Models
{
    public class QuestionCategory
    {
       
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string Description { get; set; } = string.Empty;

        public ICollection<Question> questions { get; set; }
    }
}
