using TrustSeal.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrustSeal.Models
{
    public class DocumentPath {
        public int Id {get;set;}
        public string BaseUrl {get;set;} = "C:\\Uploads";
    }
}