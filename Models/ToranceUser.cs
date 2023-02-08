using Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.Common.Interfaces;
using Models.WeldingRodRecord;
using System.ComponentModel.DataAnnotations;

namespace Models;

// Add profile data for application users by adding properties to the TimeKeepingUser class
[Index(nameof(AccessCode), IsUnique = true)]
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
}

