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

namespace ViewModels.WeldingRodRecord.WRRLog
{
    public class WRRLogModifyViewModel : BaseUpdateVM, IBaseCrudViewModel, IIdentitifier
    {
        [Display(Name = "Date Rod Returned")]
        public DateTime DateRodReturned { get; set; } = DateTime.Now;
        [Display(Name = "Calibration Date")]
        public DateTime CalibrationDate { get; set; } = DateTime.Now;
        [Required]
        [Display(Name = "Fume Control Used", Prompt = "Add Fume Control Used")]
        public FumeControlUsedCatalog FumeControlUsed { get; set; }
        [Display(Prompt = "Add Twr")]
        public string Twr { get; set; }
        [EmailAddress]
        [Display(Prompt = "Add Email")]
        [Remote(action: "ValidateWRRLogEmail", controller: "WRRLog", AdditionalFields = "Id,Email", ErrorMessage = "Email already in use.")]
        public string? Email { get; set; }
        [Display(Name = "Rod Checked Out")]
        public DateTime RodCheckedOut { get; set; } = DateTime.Now;
        [Display(Name = "Rod Checked Out lbs", Prompt = "Add Rod Checked Out lbs")]
        [Range(1, float.MaxValue, ErrorMessage = "The Rod Checked Out lbs must be greater than zero.")]
        public double RodCheckedOutLbs { get; set; }
        [Display(Name = "Rod Returned Waste lbs", Prompt = "Add Rod Returned Waste lbs")]
        [Range(1, float.MaxValue, ErrorMessage = "The Rod Returned Waste lbs must be greater than zero.")]
        public double? RodReturnedWasteLbs { get; set; }

        public DepartmentBriefViewModel Department { get; set; } = new DepartmentBriefViewModel();

        public EmployeeBriefViewModel Employee { get; set; } = new EmployeeBriefViewModel();

        public UnitBriefViewModel Unit { get; set; } = new UnitBriefViewModel();
        public ContractorBriefViewModel Contractor { get; set; } = new ContractorBriefViewModel();

        public RodTypeBriefViewModel RodType { get; set; } = new RodTypeBriefViewModel();

        public WeldMethodBriefViewModel WeldMethod { get; set; } = new WeldMethodBriefViewModel();

        public LocationBriefViewModel Location { get; set; } = new LocationBriefViewModel();

    }
}
