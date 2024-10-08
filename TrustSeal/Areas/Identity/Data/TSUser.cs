using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using TrustSeal.Models;

namespace TrustSeal.Areas.Identity.Data;

// Add profile data for application users by adding properties to the TSUser class
public class TSUser : IdentityUser
{
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    public ICollection<Business> UserBusinesses { get; set; }
}

