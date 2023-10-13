using ViewModels.Shared;
using System.ComponentModel.DataAnnotations;
using ViewModels.WeldingRodRecord;
using ViewModels.Common.Unit;
using ViewModels.Common.Company;
using ViewModels.TimeOnTools.Shift;
using ViewModels.Authentication;
using Enums;
using Helpers.Extensions;
using System.Globalization;
using ViewModels.Authentication.User;
using ViewModels.Common.Department;
using ViewModels.TimeOnTools.PermitType;
using ViewModels.TimeOnTools.ReworkDelay;
using ViewModels.TimeOnTools.ShiftDelay;
using ViewModels.TimeOnTools.StartOfWorkDelay;
using ViewModels.TimeOnTools;
using Helpers.Datetime;

namespace ViewModels.OverrideLogs.ORLog
{
    public class ORLogDetailViewModel : LogCommonDetailViewModel, IORLogCostDetailView
    {
        public long Id { get; set; }

        public DateTime CreatedOn { get; set; }
        [Display(Name = "Submitted")]
        public string FormattedCreatedOn
        {
            get
            {
                return CreatedOn.FormatDatetimeInPST();
            }
        }
        [Display(Name = "Submitted Time")]
        public string FormattedCreatedTime
        {
            get
            {
                return CreatedOn.FormatTimeInPST();
            }
        }

        [Display(Name = "Submitted Date")]
        public string FormattedCreatedDate
        {
            get
            {
                return CreatedOn.FormatDateInPST();
            }
        }

        [Display(Name = "Work Date")]
        public DateTime WorkCompletedDate { get; set; } = DateTime.UtcNow;
        public string FormattedDateOfWorkCompleted
        {
            get
            {
                return WorkCompletedDate.FormatDate();
            }
        }
        public double TotalCost { get; set; }
        public double TotalHours { get; set; }
        public double TotalHeadCount { get; set; }
        public string Description { get; set; }

        [Display(Name = "Workscope")]
        public string? WorkScope { get; set; }

        [Display(Name = "PO Number")]

        [Range(1, long.MaxValue, ErrorMessage = "The PO Number must be greater than zero.")]
        public long PoNumber { get; set; }



        public UnitBriefViewModel Unit { get; set; } = new UnitBriefViewModel();

        public ShiftBriefViewModel Shift { get; set; } = new ShiftBriefViewModel();

        public ReasonForRequestBriefViewModel ReasonForRequest { get; set; } = new ReasonForRequestBriefViewModel();

        public CraftRateBriefViewModel CraftRate { get; set; } = new CraftRateBriefViewModel();


        public DelayReasonCatalog? DelayReason { get; set; }
        public StartOfWorkDelayBriefViewModel StartOfWorkDelay { get; set; } = new StartOfWorkDelayBriefViewModel();
        public ShiftDelayBriefViewModel ShiftDelay { get; set; } = new ShiftDelayBriefViewModel();

        public ReworkDelayBriefViewModel ReworkDelay { get; set; } = new ReworkDelayBriefViewModel();
        public DelayTypeBriefViewModel DelayType { get; set; } = new DelayTypeBriefViewModel();
        public EmployeeBriefViewModel Employee { get; set; } = new(false,"");

        public CompanyBriefViewModel Company { get; set; } = new();

        public List<ORLogCostViewModel> Costs { get; set; } = new List<ORLogCostViewModel>();

        public DepartmentBriefViewModel Department { get; set; } = new DepartmentBriefViewModel();

        [Display(Name = "Override Reason")]
        public string? Reason { get; set; }

        public string PossibleApprovers { get; set; }

        [Display(Name = "Employee Name(s)")]
        public string EmployeeNames { get; set; }

        [Display(Name = "Uploaded Form")]
        public string ClippedEmployeesUrl { get; set; }

        public string DomainUrl { get; set; }

        public string FormattedClippedEmployeeUrl { get => string.IsNullOrEmpty(ClippedEmployeesUrl) ? "" : $"{DomainUrl}{ClippedEmployeesUrl}"; }
    }
}
