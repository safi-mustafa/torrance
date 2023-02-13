using Enums;
using Microsoft.AspNetCore.Mvc;
using Models.Common.Interfaces;
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
    public class WRRLogCreateViewModel : BaseCreateVM, IBaseCrudViewModel, IApprove
    {
        public DateTime DateRodReturned { get; set; } = DateTime.Now;
        public DateTime CalibrationDate { get; set; } = DateTime.Now;
        [Required]
        public FumeControlUsedCatalog FumeControlUsed { get; set; }

        [Display(Name = "Workscope")]
        public string? WorkScope { get; set; }
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
        public DateTime RodCheckedOut { get; set; } = DateTime.Now;
        [Range(1, float.MaxValue, ErrorMessage = "The Rod Checked Out lbs must be greater than zero.")]
        public double RodCheckedOutLbs { get; set; }
        [Range(1, float.MaxValue, ErrorMessage = "The Rod Returned Waste lbs must be greater than zero.")]
        public double? RodReturnedWasteLbs { get; set; }
        public Status Status { get; set; }

        public DepartmentBriefViewModel Department { get; set; } = new DepartmentBriefViewModel();

        public EmployeeBriefViewModel Employee { get; set; } = new EmployeeBriefViewModel();

        public UnitBriefViewModel Unit { get; set; } = new UnitBriefViewModel();
        public RodTypeBriefViewModel RodType { get; set; } = new RodTypeBriefViewModel();

        public WeldMethodBriefViewModel WeldMethod { get; set; } = new WeldMethodBriefViewModel();

        public LocationBriefViewModel Location { get; set; } = new LocationBriefViewModel();
        public ContractorBriefViewModel Contractor { get; set; } = new ContractorBriefViewModel();
        public ApproverBriefViewModel Approver { get; set; } = new ApproverBriefViewModel(false);

        public CompanyBriefViewModel Company { get; set; } = new CompanyBriefViewModel();
    }
}
