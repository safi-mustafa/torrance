using System.ComponentModel.DataAnnotations;
using ViewModels.Authentication;
using ViewModels.Authentication.User;
using ViewModels.Common.Company;
using ViewModels.Common.Unit;
using ViewModels.Shared;
using ViewModels.TimeOnTools.Shift;
using ViewModels.WeldingRodRecord;

namespace ViewModels.OverrideLogs.ORLog
{
    public class ORLogCreateViewModel : BaseCreateVM, IBaseCrudViewModel
    {

        [Display(Name = "Completed")]
        public DateTime WorkCompletedDate { get; set; } = DateTime.Now;

        [Display(Name = "Hours")]
        [Range(1, long.MaxValue, ErrorMessage = "The Hours must be greater than zero.")]
        public int OverrideHours { get; set; }

        [Display(Name = "PO Number")]

        [Range(1, long.MaxValue, ErrorMessage = "The PO Number must be greater than zero.")]
        public long PoNumber { get; set; }
        [Required]
        public string Description { get; set; }

        public UnitBriefViewModel Unit { get; set; } = new();

        public ShiftBriefViewModel Shift { get; set; } = new ShiftBriefViewModel();

        public ReasonForRequestBriefViewModel ReasonForRequest { get; set; } = new ReasonForRequestBriefViewModel();

        //public CraftRateBriefViewModel CraftRate { get; set; } = new CraftRateBriefViewModel();

        public CraftSkillBriefViewModel CraftSkill { get; set; } = new CraftSkillBriefViewModel();

        public OverrideTypeBriefViewModel OverrideType { get; set; } = new OverrideTypeBriefViewModel();

        public EmployeeBriefViewModel Employee { get; set; } = new();
        public CompanyBriefViewModel Company { get; set; } = new();

        public ApproverBriefViewModel Approver { get; set; } = new ApproverBriefViewModel(true);
    }
}
