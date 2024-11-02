using TrustSeal.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrustSeal.Models
{
    public class Seal {
        public string Id {get;set;}

        public DateTime CreatedAt {get;set;}

        public DateTime UpdatedAt {get;set;}

        public DateTime ExpiresAt {get;set;}

        public DateTime RenewedAt {get;set;}

        public int BusinessId {get;set;}

        [ForeignKey("BusinessID")]
        public Business Business {get;set;}
    }
}