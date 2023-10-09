using Enums;
using System.ComponentModel.DataAnnotations;
using ViewModels.Authentication.User;
using ViewModels.Common.Company;
using ViewModels.Common.Contractor;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.OverrideLogs;
using ViewModels.Shared;
using ViewModels.TimeOnTools.PermittingIssue;
using ViewModels.TimeOnTools.PermitType;
using ViewModels.TimeOnTools.Shift;
using ViewModels.WeldingRodRecord;

namespace ViewModels.TimeOnTools.TOTLog
{
    public class TOTLogCreateViewModel : LogDelayReasonCreateVM, IBaseCrudViewModel, ITOTLogNotificationViewModel
    {
        public DateTime? Date { get; set; } = DateTime.UtcNow;
        public string? Twr
        {
            get
            {
                return TWRModel.Name + "-" + TWRModel.NumericPart.id + "-" + TWRModel.AlphabeticPart.id + "-" + TWRModel.Text;
            }
        }
        [Required(ErrorMessage = "*")]
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

        [Display(Name = "Total Manhours", Prompt = "Add Man Hours")]
        [Range(1, long.MaxValue, ErrorMessage = "The Man Hours must be greater than zero.")]
        public long ManHours { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartOfWork { get; set; } = DateTime.UtcNow;
        [Display(Name = "Time Requested")]
        public TimeSpan? TimeRequested { get; set; } = TimeSpan.Zero;
        [Display(Name = "Time Signed")]
        public TimeSpan? TimeSigned { get; set; } = TimeSpan.Zero;
        public string? Comment { get; set; }
        public ReasonForRequestBriefViewModel ReasonForRequest { get; set; } = new ReasonForRequestBriefViewModel(false, "");
        [Required]
        [Display(Name = "Description", Prompt = "Add Description")]
        public string? JobDescription { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = "The Man Power must be greater than zero.")]
        [Display(Name = "Total Head Count")]
        public long ManPowerAffected { get; set; }
        [Display(Name = "Equipment No")]
        //[Required(ErrorMessage ="The field Equipment No is required.")]
        public string? EquipmentNo { get; set; }

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
        [Required]
        public string? Foreman { get; set; }

        public EmployeeBriefViewModel Employee { get; set; } = new EmployeeBriefViewModel();

        public CompanyBriefViewModel Company { get; set; } = new CompanyBriefViewModel();

    }
}
