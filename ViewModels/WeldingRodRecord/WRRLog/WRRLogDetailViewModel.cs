using Enums;
using Helpers.Datetime;
using Models.Common.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ViewModels.Authentication.User;
using ViewModels.Common.Company;
using ViewModels.Common.Contractor;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.Shared;
using ViewModels.TimeOnTools.TOTLog;
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
        public DateTime Date { get { return CreatedOn; } }
        public string FormattedDate
        {
            get
            {
                return CreatedOn.FormatDateInPST();
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
                return DateRodReturned.FormatDateInPST();
            }
        }
        [DisplayName("Calibration Date")]
        public DateTime CalibrationDate { get; set; }
        public string FormattedCalibrationDate
        {
            get
            {
                return CalibrationDate.FormatDateInPST();
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
                return RodCheckedOut.FormatDateInPST();
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
