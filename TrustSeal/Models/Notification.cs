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

        public int? BsAnswerId {get;set;}

        [ForeignKey("BsAnswerId")]
        public BsAnswer BsAnswer {get;set;}

        public int? QuestionId {get; set;}

        [ForeignKey("QuestionId")]
        public Question Question {get;set;}

        public int? QuestionCategoryId {get;set;}

        [ForeignKey("QuestionCategoryId")]
        public QuestionCategory QuestionCategory {get;set;}

        public int? SealId {get;set;}

        [ForeignKey("SealId")]
        public Seals Seals {get;set;}

        public int? ApplicationTrackingId {get;set;}

        [ForeignKey("ApplicationTrackingId")]
        public ApplicationTracking ApplicationTracking {get;set;}

    }
}