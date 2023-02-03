using Enums;
using Microsoft.AspNetCore.Identity;
using Models.Common;
using Models.Common.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

// Add profile data for application users by adding properties to the TimeKeepingUser class
public class ToranceUser : IdentityUser<long>, IBaseModel
{
    public bool IsDeleted { get; set; }
    public ActiveStatus ActiveStatus { get; set; }
    public DateTime CreatedOn { get; set; }
    public long CreatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }
    public long UpdatedBy { get; set; }
}

