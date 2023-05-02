using Enums;
using Helpers.Extensions;
using Models.Common.Interfaces;
using Select2.Model;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ViewModels.Authentication;
using ViewModels.Authentication.User;
using ViewModels.Common.Company;
using ViewModels.Common.Contractor;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.Shared;
using ViewModels.TimeOnTools.TOTLog;
using ViewModels.WeldingRodRecord.Employee;
using ViewModels.WeldingRodRecord.Location;
using ViewModels.WeldingRodRecord.RodType;
using ViewModels.WeldingRodRecord.WeldMethod;

namespace ViewModels.WeldingRodRecord.WRRLog
{
    public class WRRLogDetailViewModel : LogCommonDetailViewModel, IApprove
    {
        public WRRLogDetailViewModel()
        {
            Approver = new ApproverBriefViewModel();
        }
        public long Id { get; set; }
        public DateTime CreatedOn { get; set; }

        public string FormattedCreatedOn
        {
            get
            {
                return CreatedOn.Date.ToString("MM/dd/yyyy");
            }
        }
        public DateTime Date { get { return CreatedOn; } }
        public string FormattedDate
        {
            get
            {
                return Date.Date.ToString("MM/dd/yyyy");
            }
        }
        [Display(Name = "Workscope")]
        public string? WorkScope { get; set; }
        [DisplayName("Returned")]
        public DateTime DateRodReturned { get; set; }
        public string FormattedDateRodReturned
        {
            get
            {
                return DateRodReturned.Date.ToString("MM/dd/yyyy");
            }
        }
        [DisplayName("Calibration Date")]
        public DateTime CalibrationDate { get; set; }
        public string FormattedCalibrationDate
        {
            get
            {
                return CalibrationDate.Date.ToString("MM/dd/yyyy");
            }
        }
        [Required]
        [DisplayName("Fume Control Used")]
        public FumeControlUsedCatalog FumeControlUsed { get; set; }
        public string Twr { get; set; }

        public TWRViewModel TWRModel { get; set; } = new TWRViewModel();
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Checked Out")]
        public DateTime RodCheckedOut { get; set; }
        public string FormattedRodCheckedOut
        {
            get
            {
                return RodCheckedOut.Date.ToString("MM/dd/yyyy");
            }
        }
        [DisplayName("Rod Checked Out lbs")]
        public double RodCheckedOutLbs { get; set; }
        [DisplayName("Rod Returned Waste lbs")]
        public double RodReturnedWasteLbs { get; set; }

        public DepartmentBriefViewModel Department { get; set; } = new DepartmentBriefViewModel();

        public EmployeeBriefViewModel Employee { get; set; } = new EmployeeBriefViewModel();
        public ContractorBriefViewModel Contractor { get; set; } = new ContractorBriefViewModel();
        public UnitBriefViewModel Unit { get; set; } = new UnitBriefViewModel();

        public RodTypeBriefViewModel RodType { get; set; } = new RodTypeBriefViewModel();

        public WeldMethodBriefViewModel WeldMethod { get; set; } = new WeldMethodBriefViewModel();
        public LocationBriefViewModel Location { get; set; } = new LocationBriefViewModel();

        public CompanyBriefViewModel Company { get; set; } = new CompanyBriefViewModel();

        public string PossibleApprovers { get; set; }
    }
}
