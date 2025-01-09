using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrustSeal.Areas.Identity.Data;

namespace TrustSeal.Models 
{
    public class SupportAttachment {
        public int Id {get;set;}

        public string FileUrl {get;set;}

        public string DisplayName {get;set;}

        public DateTime CreatedAt {get;set;}

        public DateTime UpdatedAt {get;set;}

        public int? SupportMessageId {get;set;}
        public Support SupportMessage {get;set;}
    }
}