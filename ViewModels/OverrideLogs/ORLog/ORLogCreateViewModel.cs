using Enums;
using Models.Common.Interfaces;
using Models.OverrideLogs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ViewModels.AppSettings.Map;
using ViewModels.Authentication;
using ViewModels.Common.Contractor;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.Shared;
using ViewModels.TomeOnTools.PermittingIssue;
using ViewModels.TomeOnTools.PermitType;
using ViewModels.TomeOnTools.ReworkDelay;
using ViewModels.TomeOnTools.Shift;
using ViewModels.TomeOnTools.ShiftDelay;
using ViewModels.WeldingRodRecord;
using ViewModels.WeldingRodRecord.Employee;

namespace ViewModels.OverrideLogs.ORLog
{
    public class ORLogCreateViewModel : BaseCreateVM, IBaseCrudViewModel
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

        public EmployeeMultiselectBriefViewModel Employees { get; set; } = new();
    }
}
