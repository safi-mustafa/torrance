using System.ComponentModel.DataAnnotations;

namespace Enums
{
    public enum Status
    {
        [Display(Name = "Submitted")]
        Pending,
        Approved,
        Rejected,
        //for FCOLogs
        Partial,
        Archived = 100,
        [Display(Name = "In Process")]
        InProcess
    }

    public enum StatusSearchEnum
    {
        Approved = 1,
        Rejected = 2,
        Archived = 100
    }

    public enum ApprovalStatus
    {
        Pending = 0,
        [Display(Name = "In Process")]
        InProcess = 3
    }
}
