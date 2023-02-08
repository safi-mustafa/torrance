using Enums;
using Models.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ViewModels.AppSettings.Map;
using ViewModels.Authentication;
using ViewModels.Common.Company;
using ViewModels.Common.Contractor;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.OverrideLogs;
using ViewModels.Shared;
using ViewModels.TimeOnTools.PermittingIssue;
using ViewModels.TimeOnTools.PermitType;
using ViewModels.TimeOnTools.ReworkDelay;
using ViewModels.TimeOnTools.Shift;
using ViewModels.TimeOnTools.ShiftDelay;
using ViewModels.WeldingRodRecord;

namespace ViewModels.TimeOnTools.TOTLog
{
    public class TOTLogCreateViewModel : BaseCreateVM, IBaseCrudViewModel
    {
        public DateTime? Date { get; set; } = DateTime.Now;
        public string? Twr
        {
            get
            {
                return TWRModel.Name + "-" + TWRModel.NumericPart.id + "-" + TWRModel.AlphabeticPart.id + "-" + TWRModel.Text;
            }
        }
        [Required(ErrorMessage = "*")]
        public TWRViewModel TWRModel { get; set; } = new TWRViewModel();

        [Display(Name = "Total Manhours", Prompt = "Add Man Hours")]
        [Range(1, long.MaxValue, ErrorMessage = "The Man Hours must be greater than zero.")]
        public long ManHours { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartOfWork { get; set; } = DateTime.Now;
        [Display(Name = "Time Requested")]
        public TimeSpan? TimeRequested { get; set; } = TimeSpan.Zero;
        [Display(Name = "Time Signed")]
        public TimeSpan? TimeSigned { get; set; } = TimeSpan.Zero;
        public string? Comment { get; set; }
        public ReasonForRequestBriefViewModel ReasonForRequest { get; set; } = new ReasonForRequestBriefViewModel();
        [Display(Name = "Description", Prompt = "Add Description")]
        public string JobDescription { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = "The Man Power must be greater than zero.")]
        [Display(Name = "Total Head Count")]
        public long ManPowerAffected { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = "The Equipment No must be greater than zero.")]
        public long EquipmentNo { get; set; }

        //[Range(1, double.MaxValue, ErrorMessage = "The Hours Delayed must be greater than zero.")]
        public double? HoursDelayed { get; set; }
        public Status Status { get; set; }

        public DepartmentBriefViewModel Department { get; set; } = new DepartmentBriefViewModel();

        public UnitBriefViewModel Unit { get; set; } = new UnitBriefViewModel();

        public ContractorBriefViewModel Contractor { get; set; } = new ContractorBriefViewModel();

        public ShiftDelayBriefViewModel ShiftDelay { get; set; } = new ShiftDelayBriefViewModel();

        public ReworkDelayBriefViewModel ReworkDelay { get; set; } = new ReworkDelayBriefViewModel();

        public PermitTypeBriefViewModel PermitType { get; set; } = new PermitTypeBriefViewModel();
        public DelayTypeBriefViewModel DelayType { get; set; } = new DelayTypeBriefViewModel();
        public ShiftBriefViewModel Shift { get; set; } = new ShiftBriefViewModel();
        public PermittingIssueBriefViewModel PermittingIssue { get; set; } = new PermittingIssueBriefViewModel();

        public ApproverBriefViewModel Approver { get; set; } = new ApproverBriefViewModel(true);

        public UserBriefViewModel Foreman { get; set; } = new UserBriefViewModel();

        public EmployeeBriefViewModel Employee { get; set; } = new EmployeeBriefViewModel();

        public CompanyBriefViewModel Company { get; set; } = new CompanyBriefViewModel();

    }
}
