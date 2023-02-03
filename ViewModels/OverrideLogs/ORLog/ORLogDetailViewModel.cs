using ViewModels.Shared;
using System.ComponentModel.DataAnnotations;
using ViewModels.WeldingRodRecord;
using ViewModels.Common.Unit;
using ViewModels.Common.Company;
using ViewModels.TimeOnTools.Shift;

namespace ViewModels.OverrideLogs.ORLog
{
    public class ORLogDetailViewModel : BaseCrudViewModel
    {
        public long Id { get; set; }

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

        [Display(Name = "Work Completed Date")]
        public DateTime WorkCompletedDate { get; set; } = DateTime.Now;
        public string FormattedDateOfWorkCompleted
        {
            get
            {
                return WorkCompletedDate.Date.ToString("MM/dd/yyyy");
            }
        }

        public string Description { get; set; }

        [Display(Name = "Override Hours")]
        public int OverrideHours { get; set; }

        public float FormattedCraft
        {
            get
            {
                if(CraftRate != null)
                {
                    return CraftRate.Rate * OverrideHours;
                }
                return 0;
            }
        }

        [Display(Name = "PO Number")]

        [Range(1, long.MaxValue, ErrorMessage = "The PO Number must be greater than zero.")]
        public long PONumber { get; set; }

        public UnitBriefViewModel Unit { get; set; } = new UnitBriefViewModel();

        public ShiftBriefViewModel Shift { get; set; } = new ShiftBriefViewModel();

        public ReasonForRequestBriefViewModel ReasonForRequest { get; set; } = new ReasonForRequestBriefViewModel();

        public CraftRateBriefViewModel CraftRate { get; set; } = new CraftRateBriefViewModel();

        public CraftSkillBriefViewModel CraftSkill { get; set; } = new CraftSkillBriefViewModel();

        public OverrideTypeBriefViewModel OverrideType { get; set; } = new OverrideTypeBriefViewModel();

        public EmployeeBriefViewModel Requester { get; set; } = new();

        public CompanyBriefViewModel Company { get; set; } = new();
    }
}
