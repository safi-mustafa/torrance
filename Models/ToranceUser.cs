using Enums;
using Helpers.Models.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.Common;
using Models.Common.Interfaces;
using Models.WeldingRodRecord;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

// Add profile data for application users by adding properties to the TimeKeepingUser class
[Index(nameof(AccessCode), IsUnique = false)]
public class ToranceUser : IdentityUser<long>, IBaseModel
{
    public bool IsDeleted { get; set; }
    public ActiveStatus ActiveStatus { get; set; }
    public DateTime CreatedOn { get; set; }
    public long CreatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }
    public long UpdatedBy { get; set; }
    public string? DeviceId { get; set; }
    //  [Required]
    //  [StringLength(4, ErrorMessage = "AccessCode must be of 4-digits.", MinimumLength = 4)]
    public string? AccessCode { get; set; }


    [Required]
    [MaxLength(300, ErrorMessage = "Full Name cannot be greater than 300 characters.")]
    public string FullName { get; set; }
    [ForeignKey("Company")]
    public long? CompanyId { get; set; }
    public Company? Company { get; set; }

    public bool ChangePassword { get; set; }
}

