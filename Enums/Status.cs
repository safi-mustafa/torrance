using System.ComponentModel.DataAnnotations;

namespace Enums
{
    public enum Status
    {
        [Display(Name = "Pending")]
        Pending,
        Approved,
        Rejected,
        [Display(Name = "In Process")]
        InProcess
    }

    public enum StatusSearchEnum
    {
        Approved = 1,
        Rejected = 2
    }

    public enum ApprovalStatus
    {
        Pending = 0,
        [Display(Name = "In Process")]
        InProcess = 3
    }
}
