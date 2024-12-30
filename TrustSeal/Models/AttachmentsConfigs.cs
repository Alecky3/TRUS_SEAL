using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrustSeal.Models
{
    public class AttachmentConfigs{
        public int Id {get;set;}

        public string Category {get;set;}

        public string ForWhichField {get;set;}

        public bool Required {get;set;} = false;

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

    }
}