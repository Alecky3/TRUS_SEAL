using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrustSeal.Models
{
   public  class BusinessAttachment {
        public int Id {get;set;}
        public string FileReference {get;set;}

        public int? BusinessId {get;set;}

        [ForeignKey("BusinessId")]
        public Business Business {get; set;}
        public int? BsAnswerId {get;set;}
        [ForeignKey("BsAnswerId")]
        public BsAnswer BsAnswer {get;set;}

        public List<Notification> Notifications {get;set;}= new List<Notification>();

    }
}