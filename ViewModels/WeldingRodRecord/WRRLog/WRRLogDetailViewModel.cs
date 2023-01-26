using Enums;
using Select2.Model;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ViewModels.Common.Contractor;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.Shared;
using ViewModels.WeldingRodRecord.Employee;
using ViewModels.WeldingRodRecord.Location;
using ViewModels.WeldingRodRecord.RodType;
using ViewModels.WeldingRodRecord.WeldMethod;

namespace ViewModels.WeldingRodRecord.WRRLog
{
    public class WRRLogDetailViewModel : BaseCrudViewModel
    {
        public long Id { get; set; }
        [DisplayName("Date Rod Returned")]
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
        [DisplayName("Twr")]
        public string Twr { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Rod Checked Out")]
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

        public Status Status { get; set; }

        public DepartmentBriefViewModel Department { get; set; } = new DepartmentBriefViewModel();

        public EmployeeBriefViewModel Employee { get; set; } = new EmployeeBriefViewModel();
        public ContractorBriefViewModel Contractor { get; set; } = new ContractorBriefViewModel();
        public UnitBriefViewModel Unit { get; set; } = new UnitBriefViewModel();

        public RodTypeBriefViewModel RodType { get; set; } = new RodTypeBriefViewModel();

        public WeldMethodBriefViewModel WeldMethod { get; set; } = new WeldMethodBriefViewModel();

        public LocationBriefViewModel Location { get; set; } = new LocationBriefViewModel();
    }
}
