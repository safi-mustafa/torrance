using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Models.Common.Interfaces;
using ViewModels.Shared;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.WeldingRodRecord.RodType;
using ViewModels.WeldingRodRecord.WeldMethod;
using ViewModels.WeldingRodRecord.Location;
using Microsoft.AspNetCore.Mvc;
using ViewModels.WeldingRodRecord.Employee;
using ViewModels.Common.Contractor;
using Enums;
using ViewModels.Authentication;
using ViewModels.Authentication.User;
using ViewModels.TimeOnTools.TOTLog;
using ViewModels.Common.Company;

namespace ViewModels.WeldingRodRecord.WRRLog
{
    public class WRRLogModifyViewModel : BaseUpdateVM, IBaseCrudViewModel, IIdentitifier, IApprove, IWRRLogNotificationViewModel
    {
        [Display(Name = "Rod Returned")]
        [Required]
        public DateTime? DateRodReturned { get; set; }
        [Required]
        [Display(Name = "Calibration Date")]
        public DateTime? CalibrationDate { get; set; }
        [Required]
        [Display(Name = "Fume Control Used", Prompt = "Add Fume Control Used")]
        public FumeControlUsedCatalog FumeControlUsed { get; set; }

        [Display(Name = "Workscope")]
        public string? WorkScope { get; set; }
        [Display(Prompt = "Add Twr")]
        public string? Twr
        {
            get
            {
                return TWRModel.Name + "-" + TWRModel.NumericPart.id + "-" + TWRModel.AlphabeticPart.id + "-" + TWRModel.Text;
            }
        }
        public TWRViewModel TWRModel { get; set; } = new TWRViewModel();
        [EmailAddress]
        [Display(Prompt = "Add Email")]
        //[Remote(action: "ValidateWRRLogEmail", controller: "WRRLog", AdditionalFields = "Id,Email", ErrorMessage = "Email already in use.")]
        public string? Email { get; set; }
        [Required]
        [Display(Name = "Rod Checked Out")]
        public DateTime? RodCheckedOut { get; set; }
        [Display(Name = "Rod Checked Out lbs", Prompt = "Add Rod Checked Out lbs")]
        [Range(1, float.MaxValue, ErrorMessage = "The Rod Checked Out lbs must be greater than zero.")]
        public double RodCheckedOutLbs { get; set; }
        [Required]
        [Display(Name = "Rod Returned Waste lbs", Prompt = "Add Rod Returned Waste lbs")]
        [Range(1, float.MaxValue, ErrorMessage = "The Rod Returned Waste lbs must be greater than zero.")]
        public double RodReturnedWasteLbs { get; set; }
        public Status Status { get; set; }
        [Display(Name = "Is Archived")]
        public bool IsArchived { get; set; }

        public DepartmentBriefViewModel Department { get; set; } = new DepartmentBriefViewModel(true);

        private EmployeeBriefViewModel? _employee;
        public EmployeeBriefViewModel? Employee { get => _employee == null ? new EmployeeBriefViewModel() : _employee; set => _employee = value; }


        public UnitBriefViewModel Unit { get; set; } = new UnitBriefViewModel();

        public ContractorBriefViewModel Contractor { get; set; } = new ContractorBriefViewModel();

        public RodTypeBriefViewModel RodType { get; set; } = new RodTypeBriefViewModel();

        public WeldMethodBriefViewModel WeldMethod { get; set; } = new WeldMethodBriefViewModel();

        public LocationBriefViewModel Location { get; set; } = new LocationBriefViewModel();

        public ApproverBriefViewModel Approver { get; set; } = new ApproverBriefViewModel();

        public CompanyBriefViewModel Company { get; set; } = new CompanyBriefViewModel();
    }
}
