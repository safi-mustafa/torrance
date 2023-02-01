using Enums;
using System.ComponentModel;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.Shared;
using ViewModels.Common.Contractor;
using ViewModels.TomeOnTools.ShiftDelay;
using ViewModels.TomeOnTools.ReworkDelay;
using ViewModels.TomeOnTools.PermitType;
using ViewModels.TomeOnTools.Shift;
using ViewModels.Authentication;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using ViewModels.AppSettings.Map;
using ViewModels.TomeOnTools.PermittingIssue;
using ViewModels.WeldingRodRecord.Employee;

namespace ViewModels.OverrideLogs.ORLog
{
    public class ORLogDetailViewModel : BaseCrudViewModel
    {
        public long Id { get; set; }
        public string Requester { get; set; }

        [Display(Name = "Requester Email")]
        [EmailAddress]
        public string RequesterEmail { get; set; }

        [Display(Name = "Date Submitted")]
        public DateTime DateSubmitted { get; set; } 
        public string FormattedDateSubmitted
        {
            get
            {
                return DateSubmitted.Date.ToString("MM/dd/yyyy");
            }
        }

        [Display(Name = "Time Submitted")]
        public TimeSpan TimeSubmitted { get; set; }
        public string FormattedTimeSubmitted
        {
            get
            {
                return TimeSubmitted.ToString();
            }
        }

        [Display(Name = "Date of Work Completed")]
        public DateTime DateOfWorkCompleted { get; set; } = DateTime.Now;
        public string FormattedDateOfWorkCompleted
        {
            get
            {
                return DateOfWorkCompleted.Date.ToString("MM/dd/yyyy");
            }
        }

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
        public List<EmployeeBriefViewModel> Employees { get; set; } = new List<EmployeeBriefViewModel>();
    }
}
