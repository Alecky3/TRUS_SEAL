using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrustSeal.Models
{
   public  class BusinessAttachment {
        public int Id {get;set;}
        public string FileReference {get;set;}

        public int BusinessID {get;set;}
        public int QuestionID {get;set;}
        
        [ForeignKey("BusinessID,QuestionID")]
        public BsAnswer BsAnswer {get;set;} = null!;

    }
}