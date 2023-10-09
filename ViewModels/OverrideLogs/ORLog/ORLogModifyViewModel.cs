using System.ComponentModel.DataAnnotations;
using Models.Common.Interfaces;
using ViewModels.Shared;
using ViewModels.WeldingRodRecord;
using ViewModels.Common.Unit;
using ViewModels.Common.Company;
using ViewModels.TimeOnTools.Shift;
using ViewModels.Authentication;
using ViewModels.Authentication.User;
using Models.Common;
using ViewModels.Common.Department;
using Enums;
using ViewModels.TimeOnTools.PermitType;
using ViewModels.TimeOnTools.ReworkDelay;
using ViewModels.TimeOnTools.ShiftDelay;
using ViewModels.TimeOnTools.StartOfWorkDelay;
using ViewModels.TimeOnTools;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Helpers.Attributes;

namespace ViewModels.OverrideLogs.ORLog
{
    public class ORLogModifyViewModel : BaseUpdateVM, IBaseCrudViewModel, IIdentitifier, IORLogCost, IClippedAttachment, IORLogNotificationViewModel
    {
        [Required]
        [Display(Name = "Work Date")]
        public DateTime? WorkCompletedDate { get; set; }

        [Required(ErrorMessage = "The PO Number field is required.")]
        [Display(Name = "PO Number")]
        [Range(1, long.MaxValue, ErrorMessage = "The PO Number must be greater than zero.")]
        public long PoNumber { get; set; }
        //[Required]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Workscope")]
        public string? WorkScope { get; set; }

        [Display(Name = "Is Archived")]
        public bool IsArchived { get; set; } = false;

        public UnitBriefViewModel Unit { get; set; } = new();

        public ShiftBriefViewModel Shift { get; set; } = new ShiftBriefViewModel();

        public ReasonForRequestBriefViewModel ReasonForRequest { get; set; } = new ReasonForRequestBriefViewModel(false, "");

        public EmployeeBriefViewModel Employee { get; set; } = new EmployeeBriefViewModel();
        public CompanyBriefViewModel Company { get; set; } = new CompanyBriefViewModel();
        public ApproverBriefViewModel Approver { get; set; } = new ApproverBriefViewModel(false);

        public DepartmentBriefViewModel Department { get; set; } = new DepartmentBriefViewModel(true);

        public DelayTypeBriefViewModel DelayType { get; set; } = new DelayTypeBriefViewModel(false, "");

        [ListMinCount<ORLogCostViewModel>(1)]
        public List<ORLogCostViewModel> Costs { get; set; } = new List<ORLogCostViewModel>();

        //[Required]
        [Display(Name = "Employee Name(s)")]
        public string? EmployeeNames { get; set; }

        [Display(Name = "Uplaod Form")]
        public ClipEmployeeModifyViewModel? ClippedEmployees { get; set; } = new();

        [Required]
        [Display(Name = "Override Reason")]
        public string? Reason { get; set; }



    }
}
