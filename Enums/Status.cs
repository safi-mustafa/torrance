using System.ComponentModel.DataAnnotations;

namespace Enums
{
    public enum Status
    {
        [Display(Name = "Pending")]
        Pending,
        Approved,
        Rejected,
        Partial
    }

    public enum StatusSearchEnum
    {
        Approved = 1,
        Rejected = 2
    }
}
