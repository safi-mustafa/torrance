﻿using Enums;
using System.ComponentModel;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.Shared;
using ViewModels.Common.Contractor;
using ViewModels.TimeOnTools.ShiftDelay;
using ViewModels.TimeOnTools.ReworkDelay;
using ViewModels.TimeOnTools.PermitType;
using ViewModels.TimeOnTools.Shift;
using ViewModels.Authentication;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using ViewModels.AppSettings.Map;
using ViewModels.TimeOnTools.PermittingIssue;
using ViewModels.WeldingRodRecord;
using Helpers.Extensions;
using ViewModels.Common.Company;
using ViewModels.OverrideLogs;
using ViewModels.Authentication.User;
using ViewModels.TimeOnTools.StartOfWorkDelay;

namespace ViewModels.TimeOnTools.TOTLog
{
    public class TOTLogDetailViewModel : LogCommonDetailViewModel
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string FormattedDate
        {
            get
            {
                return Date.Date.ToString("MM/dd/yyyy");
            }
        }

        public DateTime CreatedOn { get; set; }
        [Display(Name = "Submitted")]
        public string FormattedCreatedOn
        {
            get
            {
                return CreatedOn.Date.ToString("MM/dd/yyyy");
            }
        }
        [Display(Name = "Permit No")]
        public string? PermitNo { get; set; }

        [Display(Name = "Delay Description")]
        public string? DelayDescription { get; set; }
        [Display(Name = "Workscope")]
        public string? WorkScope { get; set; }
        public string Twr { get; set; }

        public TWRViewModel TWRModel { get; set; } = new TWRViewModel();

        [Display(Name = "Hours")]
        public long ManHours { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartOfWork { get; set; }
        public string FormattedStartOfWork
        {
            get
            {
                return StartOfWork.Date.ToString("MM/dd/yyyy");
            }
        }
        [Display(Name = "Time Requested")]
        public TimeSpan TimeRequested { get; set; }

        public string FormattedTimeRequested
        {
            get
            {
                return TimeRequested.ToString();
            }
        }

        [Display(Name = "Time Signed")]
        public TimeSpan TimeSigned { get; set; }
        public string FormattedTimeSigned
        {
            get
            {
                return TimeSigned.ToString();
            }
        }
        public string? Comment { get; set; }
        [Display(Name = "Reason")]
        public ReasonForRequestBriefViewModel ReasonForRequest { get; set; } = new ReasonForRequestBriefViewModel();
        [Display(Name = "Description")]
        public string JobDescription { get; set; }
        [Display(Name = "Head Count")]
        public long ManPowerAffected { get; set; }
        [Display(Name = "Total Hours")]
        public long TotalHours { get
            {
                return ManPowerAffected * ManHours;
            } }
        [Display(Name = "Equipment No")]
        public string EquipmentNo { get; set; }

        [Display(Name = "Hours Delayed")]
        public double HoursDelayed { get; set; }

        public DepartmentBriefViewModel Department { get; set; } = new DepartmentBriefViewModel();

        public UnitBriefViewModel Unit { get; set; } = new UnitBriefViewModel();

        public ContractorBriefViewModel Contractor { get; set; } = new ContractorBriefViewModel();
        public DelayReasonCatalog? DelayReason { get; set; }
        public StartOfWorkDelayBriefViewModel StartOfWorkDelay { get; set; } = new StartOfWorkDelayBriefViewModel();
        public ShiftDelayBriefViewModel ShiftDelay { get; set; } = new ShiftDelayBriefViewModel();

        public ReworkDelayBriefViewModel ReworkDelay { get; set; } = new ReworkDelayBriefViewModel();

        public PermitTypeBriefViewModel PermitType { get; set; } = new PermitTypeBriefViewModel();

        public ShiftBriefViewModel Shift { get; set; } = new ShiftBriefViewModel();
        public DelayTypeBriefViewModel DelayType { get; set; } = new DelayTypeBriefViewModel();
        public CompanyBriefViewModel Company { get; set; } = new CompanyBriefViewModel();


        public string Foreman { get; set; }
        public PermittingIssueBriefViewModel PermittingIssue { get; set; } = new PermittingIssueBriefViewModel();
        public EmployeeBriefViewModel Employee { get; set; } = new EmployeeBriefViewModel();
    }
}
