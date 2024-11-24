using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrustSeal.Models 
{
    public class Seals {
        public int Id {get;set;}

        public Guid SealCode {get;set;} = Guid.NewGuid();

        public DateTime CreatedAt {get;set;} = DateTime.Now;

        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        public DateTime ExpiresAt {get;set;} = DateTime.Now.AddYears(1);

        public DateTime RenewedAt {get;set;} = DateTime.Now;

        public Business Business {get;set;}

        public List<Notification> Notifications {get;set;} = new List<Notification>();
    }
}