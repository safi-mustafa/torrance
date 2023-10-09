using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Models.Common.Interfaces;
using ViewModels.Shared;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.Authentication;
using ViewModels.Common.Contractor;
using ViewModels.TimeOnTools.PermitType;
using ViewModels.TimeOnTools.ReworkDelay;
using ViewModels.TimeOnTools.Shift;
using ViewModels.TimeOnTools.ShiftDelay;
using Enums;
using ViewModels.AppSettings.Map;
using ViewModels.TimeOnTools.PermittingIssue;
using ViewModels.WeldingRodRecord;
using ViewModels.OverrideLogs;
using Helpers.Extensions;
using ViewModels.Common.Company;
using ViewModels.Authentication.User;
using ViewModels.TimeOnTools.StartOfWorkDelay;

namespace ViewModels.TimeOnTools.TOTLog
{
    public class TOTLogModifyViewModel : LogDelayReasonUpdateVM, IBaseCrudViewModel, IIdentitifier, ITOTLogNotificationViewModel
    {
        public DateTime? Date { get; set; } = DateTime.UtcNow;
        [Display(Name = "Twr", Prompt = "Add Twr")]
        public string? Twr
        {
            get
            {
                return TWRModel.Name + "-" + TWRModel.NumericPart.id + "-" + TWRModel.AlphabeticPart.id + "-" + TWRModel.Text;
            }
        }
        public TWRViewModel TWRModel { get; set; } = new TWRViewModel();
        [Required]
        [Display(Name = "Permit No")]
        [RegularExpression(@"^(\d{5})$", ErrorMessage = "Permit No must be of 5-digits.")]
        public string? PermitNo { get; set; }
        [Required]
        [Display(Name = "Delay Description")]
        public string? DelayDescription { get; set; }
        [Display(Name = "Workscope")]
        public string? WorkScope { get; set; }
        [Required]
        public string? Foreman { get; set; }
        [Display(Name = "Is Archived")]
        public bool IsArchived { get; set; }

        [Display(Name = "Hours", Prompt = "Add Hours")]
        [Range(1, long.MaxValue, ErrorMessage = "The Hours must be greater than zero.")]
        public long ManHours { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartOfWork { get; set; } = DateTime.UtcNow;
        [Display(Name = "Time Requested")]
        public TimeSpan? TimeRequested { get; set; } = TimeSpan.Zero;
        [Display(Name = "Time Signed")]
        public TimeSpan? TimeSigned { get; set; } = TimeSpan.Zero;
        public string? Comment { get; set; }

        [Display(Name = "Reason", Prompt = "Add Delay Reason")]
        public ReasonForRequestBriefViewModel ReasonForRequest { get; set; } = new ReasonForRequestBriefViewModel(false, "");
        [Required]
        [Display(Name = "Description", Prompt = "Add Description")]
        public string? JobDescription { get; set; }
        [Display(Name = "Head Count", Prompt = "Add Head Count")]
        [Range(1, long.MaxValue, ErrorMessage = "The Head Count must be greater than zero.")]
        public long ManPowerAffected { get; set; }
        [Display(Name = "Equipment No")]
        //[Required(ErrorMessage = "The field Equipment No is required.")]
        public string? EquipmentNo { get; set; }

        [Display(Name = "Hours Delayed", Prompt = "Add Hours Delayed")]
        //[Range(1, double.MaxValue, ErrorMessage = "The Hours Delayed must be greater than zero.")]
        public double? HoursDelayed { get; set; }
        public Status Status { get; set; }

        public DepartmentBriefViewModel Department { get; set; } = new DepartmentBriefViewModel(true);

        public UnitBriefViewModel Unit { get; set; } = new UnitBriefViewModel();

        public ContractorBriefViewModel Contractor { get; set; } = new ContractorBriefViewModel();

        public PermitTypeBriefViewModel PermitType { get; set; } = new PermitTypeBriefViewModel();


        public ShiftBriefViewModel Shift { get; set; } = new ShiftBriefViewModel();

        public PermittingIssueBriefViewModel PermittingIssue { get; set; } = new PermittingIssueBriefViewModel();

        public ApproverBriefViewModel Approver { get; set; } = new ApproverBriefViewModel();



        public EmployeeBriefViewModel Employee { get; set; } = new EmployeeBriefViewModel();

        public CompanyBriefViewModel Company { get; set; } = new CompanyBriefViewModel();
    }


}
