using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrustSeal.Areas.Identity.Data;

namespace TrustSeal.Models
{
    public class Business
    {

        public int Id { get; set; }
        [Required]
        public string LegalName { get; set; }
        public string RegistrationNumber { get; set; }
        public string TaxIdentificationNumber { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name ="Incorporation Date")]
        public DateTime IncorporationDate { get; set; }
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        [Display(Name ="Business Contact")]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }

        [Required]
        public string Website { get; set; }
        [Required]
        [Display(Name = "Primary Contact Name")]
        public string PrimaryContactName { get; set; }
        [Required]
        [Display(Name = "Primary Contact Phone")]
        public string PrimaryContactPhone { get; set; }
        [Required]
        [Display(Name = "Primary Contact Email")]
        public string PrimaryContactEmail { get; set; }
        public string BusinessType { get; set; }
        public string IndustryCategory { get; set; }
        public bool IsVerified { get; set; } = false;

        [Required]
        [Display(Name = "Submission Date")]
        [DataType(DataType.DateTime)]
        public DateTime SubmissionDate { get; set; }

        public string OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public TSUser Owner { get; set; }

    }
}
