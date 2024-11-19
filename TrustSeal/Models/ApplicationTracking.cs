using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrustSeal.Models
{
    public class ApplicationTracking {
        public int Id {get;set;}

        public string Status {get;set;}
        public string Comments {get;set;}

        public DateTime CreatedAt {get;set;}

        public DateTime UpdatedAt {get;set;}

        public bool Required {get;set;}

        public bool StepVerified {get;set;} = false;

        public bool NeedsAttention {get;set;}

        public int Order {get;set;}

        public int? BusinessId {get;set;}

        [ForeignKey("BusinessId")]
        public Business Business {get;set;}

        public List<Notification> Notifications{get;set;}
    }
}