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

namespace ViewModels.OverrideLogs.ORLog
{
    public class ORLogModifyViewModel : BaseUpdateVM, IBaseCrudViewModel, IIdentitifier, IORLogCost
    {

        [Display(Name = "Completed")]
        public DateTime WorkCompletedDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "The PO Number field is required.")]
        [Display(Name = "PO Number")]
        [Range(1, long.MaxValue, ErrorMessage = "The PO Number must be greater than zero.")]
        public long PoNumber { get; set; }
        //[Required]
        public string? Description { get; set; }

        [Display(Name = "Workscope")]
        public string? WorkScope { get; set; }

        public UnitBriefViewModel Unit { get; set; } = new();

        public ShiftBriefViewModel Shift { get; set; } = new ShiftBriefViewModel();

        public ReasonForRequestBriefViewModel ReasonForRequest { get; set; } = new ReasonForRequestBriefViewModel();

        public EmployeeBriefViewModel Employee { get; set; } = new EmployeeBriefViewModel();
        public CompanyBriefViewModel Company { get; set; } = new CompanyBriefViewModel();
        public ApproverBriefViewModel Approver { get; set; } = new ApproverBriefViewModel(false);

        public DepartmentBriefViewModel Department { get; set; } = new DepartmentBriefViewModel(true);

        public ShiftDelayBriefViewModel ShiftDelay { get; set; } = new ShiftDelayBriefViewModel();
        public DelayReasonCatalog? DelayReason { get; set; }
        public StartOfWorkDelayBriefViewModel StartOfWorkDelay { get; set; } = new StartOfWorkDelayBriefViewModel();

        public ReworkDelayBriefViewModel ReworkDelay { get; set; } = new ReworkDelayBriefViewModel();
        public DelayTypeBriefViewModel DelayType { get; set; } = new DelayTypeBriefViewModel();


        //private ApproverBriefViewModel? _approver;
        //public ApproverBriefViewModel Approver { get => _approver == null ? new ApproverBriefViewModel(false) : _approver; set => _approver = value; }

        public List<ORLogCostViewModel> Costs { get; set; } = new List<ORLogCostViewModel>();
    }
}
