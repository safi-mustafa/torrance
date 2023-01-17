using Select2.Model;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.Shared;
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
        [DisplayName("Calibration Date")]
        public DateTime CalibrationDate { get; set; }
        [Required]
        [DisplayName("Fume Control Used")]
        public string FumeControlUsed { get; set; }
        [DisplayName("Twr")]
        public string Twr { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Rod Checked Out")]
        public DateTime RodCheckedOut { get; set; }
        [DisplayName("Rod Checked Out lbs")]
        public double RodCheckedOutLbs { get; set; }
        [DisplayName("Rod Returned Waste lbs")]
        public double RodReturnedWasteLbs { get; set; }

        public DepartmentBriefViewModel Department { get; set; } = new DepartmentBriefViewModel();

        public EmployeeBriefVM Employee { get; set; } = new EmployeeBriefVM();

        public UnitBriefViewModel Unit { get; set; } = new UnitBriefViewModel();

        public RodTypeBriefViewModel RodType { get; set; } = new RodTypeBriefViewModel();

        public WeldMethodBriefViewModel WeldMethod { get; set; } = new WeldMethodBriefViewModel();

        public LocationBriefViewModel Location { get; set; } = new LocationBriefViewModel();
    }
}
