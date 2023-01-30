using System.ComponentModel.DataAnnotations;

namespace Enums
{
    public enum Status
    {
        [Display(Name = "Submitted")]
        Pending,
        Approved,
        Rejected
    }
}
