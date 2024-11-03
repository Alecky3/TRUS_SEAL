using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrustSeal.Models 
{
    public class Seals {
        public int Id {get;set;}

        public Guid SealCode {get;set;} = Guid.NewGuid();

        public DateTime CreatedAt {get;set;}

        public DateTime UpdatedAt {get;set;}

        public DateTime ExpiresAt {get;set;}

        public DateTime RenewedAt {get;set;}

        public Business Business {get;set;}

        public List<Notification> Notifications {get;set;} = new List<Notification>();
    }
}