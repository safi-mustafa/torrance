using Enums;
using Microsoft.AspNetCore.Identity;
using Models.Common.Interfaces;

namespace Models;

// Add profile data for application users by adding properties to the TimeKeepingUser class
public class ChargieUser : IdentityUser<long>, IBaseModel
{
    public bool IsDeleted { get; set; }
    public ActiveStatus ActiveStatus { get; set; }
    public DateTime CreatedOn { get; set; }
    public long CreatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }
    public long UpdatedBy { get; set; }
}

