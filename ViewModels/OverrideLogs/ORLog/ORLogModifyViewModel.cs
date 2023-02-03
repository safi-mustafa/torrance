using System.ComponentModel.DataAnnotations;
using Models.Common.Interfaces;
using ViewModels.Shared;
using ViewModels.Common.Contractor;
using ViewModels.TimeOnTools.Shift;
using ViewModels.WeldingRodRecord.Employee;
using ViewModels.WeldingRodRecord;
using Models.Common.Interfaces.OverrideLog;

namespace ViewModels.OverrideLogs.ORLog
{
    public class ORLogModifyViewModel : BaseUpdateVM, IBaseCrudViewModel, IIdentitifier, IEmployeeMultiselect
    {
        public string Requester { get; set; }

        [Display(Name = "Requester Email")]
        [EmailAddress]
        public string RequesterEmail { get; set; }

        [Display(Name = "Date Submitted")]
        public DateTime DateSubmitted { get; set; } = DateTime.Now;

        [Display(Name = "Time Submitted")]
        public TimeSpan TimeSubmitted { get; set; } = TimeSpan.MaxValue;

        [Display(Name = "Date of Work Completed")]
        public DateTime DateOfWorkCompleted { get; set; } = DateTime.Now;

        [Display(Name = "Work Scope")]
        public string? WorkScope { get; set; }

        [Display(Name = "Override Hours")]
        public int OverrideHours { get; set; }

        [Display(Name = "PO Number")]

        [Range(1, long.MaxValue, ErrorMessage = "The PO Number must be greater than zero.")]
        public long PONumber { get; set; }

        public ContractorBriefViewModel Contractor { get; set; } = new ContractorBriefViewModel();

        public ShiftBriefViewModel Shift { get; set; } = new ShiftBriefViewModel();

        public ReasonForRequestBriefViewModel ReasonForRequest { get; set; } = new ReasonForRequestBriefViewModel();

        public CraftRateBriefViewModel CraftRate { get; set; } = new CraftRateBriefViewModel();

        public CraftSkillBriefViewModel CraftSkill { get; set; } = new CraftSkillBriefViewModel();

        public OverrideTypeBriefViewModel OverrideType { get; set; } = new OverrideTypeBriefViewModel();

        public List<EmployeeBriefViewModel> Employees { get; set; } = new();

        public EmployeeMultiselectBriefViewModel EmployeeMultiselect { get; set; } = new();
    }
}
