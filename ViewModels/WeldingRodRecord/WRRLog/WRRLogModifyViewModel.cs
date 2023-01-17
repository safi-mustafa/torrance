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

namespace ViewModels.WeldingRodRecord.WRRLog
{
    public class WRRLogModifyViewModel : BaseUpdateVM, IBaseCrudViewModel, IIdentitifier
    {
        [DisplayName("Date Rod Returned")]
        public DateTime DateRodReturned { get; set; }
        [DisplayName("Calibration Date")]
        public DateTime CalibrationDate { get; set; }
        [Required]
        [DisplayName("Fume Control Used")]
        public string FumeControlUsed { get; set; }
        [DisplayName("Twr")]
        public string Twr { get; set; }
        [EmailAddress]
        [Remote(action: "ValidateEmployeeEmail", controller: "WRRLog", AdditionalFields = "Id,Email", ErrorMessage = "Email already in use.")]
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
