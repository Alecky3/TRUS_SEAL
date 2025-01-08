using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrustSeal.Areas.Identity.Data;

namespace TrustSeal.Models 
{
    public class Support {
        public int Id {get;set;}

        public string TicketNumber {get;set;}
        public string Message {get;set;}

        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}
        public bool Read {get;set;} = true;
        public string SendById {get;set;}

        public TSUser SendBy {get;set;}

        public int? ReplyToId {get;set;}

        public Support? ReplyTo {get;set;}

        public List<SupportAttachment> SupportAttachments {get;set;}

    }
}