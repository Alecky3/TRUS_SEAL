using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrustSeal.Areas.Identity.Data;

namespace TrustSeal.Models {
    public class Billing {
        public int Id {get;set;}

        public DateTime DatePaid {get;set;} = DateTime.Now;

        public string PaidById {get;set;}
        
        public TSUser UserId {get;set;}

        public Decimal Amount {get;set;}

        public String Reason {get;set;}

        public string CreatedById {get;set;}
        public String PaymentCode {get;set;}
    }
}