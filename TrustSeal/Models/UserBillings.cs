using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrustSeal.Areas.Identity.Data;

namespace TrustSeal.Models
{
    public class UserBillings {
        public int Id {get;set;}

        public string PaidById {get;set;}

        public TSUser PaidBy {get;set;}

        public Double Amount {get;set;}

        public string CreatedById {get;set;}

        public DateTime DatePaid {get;set;} = DateTime.Now;

        public string  PaymentCode {get;set;}

        public string Reason {get;set;}

        public string PaymentMethod {get;set;}

    }
}
