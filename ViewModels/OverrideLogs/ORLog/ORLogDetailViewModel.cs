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

namespace ViewModels.OverrideLogs.ORLog
{
    public class ORLogDetailViewModel : LogCommonDetailViewModel
    {
        public long Id { get; set; }

        public DateTime CreatedOn { get; set; }
        [Display(Name = "Submitted")]
        public string FormattedCreatedOn
        {
            get
            {
                return CreatedOn.Date.ToString("MM/dd/yyyy");
            }
        }


        [Display(Name = "Completed")]
        public DateTime WorkCompletedDate { get; set; } = DateTime.Now;
        public string FormattedDateOfWorkCompleted
        {
            get
            {
                return WorkCompletedDate.Date.ToString("MM/dd/yyyy");
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
        public EmployeeBriefViewModel Employee { get; set; } = new();

        public CompanyBriefViewModel Company { get; set; } = new();

        public List<ORLogCostViewModel> Costs { get; set; } = new List<ORLogCostViewModel>();

        public DepartmentBriefViewModel Department { get; set; } = new DepartmentBriefViewModel();
    }
}
