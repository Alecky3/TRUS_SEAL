using TrustSeal.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrustSeal.Models
{
    public class Notification {
        public int Id {get;set;}

        public DateTime CreatedAt {get;set;}

        public DateTime UpdateAt {get;set;}


        public string Content {get;set;}

        public int? NotificationId {get;set;}

        [ForeignKey("NotificationId")]
        public Notification ReplyToNotification{get;set;}

        public string UserId {get;set;}

        [ForeignKey("UserId")]
        public TSUser CreatedBy {get;set;}

        public int? BusinessId {get;set;}

        [ForeignKey("BusinessId")]
        public Business Business {get;set;}

        public int? BsBusinessId {get;set;}

        public int? BsQuestionId {get;set;}

        [ForeignKey("BsBusinessId,BsQuestionId")]
        public BsAnswer BsAnswer {get;set;}

        public int? QuestionId {get; set;}

        [ForeignKey("QuestionId")]
        public Question Question {get;set;}


    }
}