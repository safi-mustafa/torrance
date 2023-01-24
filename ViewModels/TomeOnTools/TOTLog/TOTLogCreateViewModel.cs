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
using ViewModels.Common.Contractor;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.Shared;
using ViewModels.TomeOnTools.PermitType;
using ViewModels.TomeOnTools.ReworkDelay;
using ViewModels.TomeOnTools.Shift;
using ViewModels.TomeOnTools.ShiftDelay;

namespace ViewModels.TomeOnTools.TOTLog
{
    public class TOTLogCreateViewModel : BaseCreateVM, IBaseCrudViewModel
    {
        public DateTime Date { get; set; } = DateTime.Now;
        public string Twr { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "The Man Hours must be greater than zero.")]
        public long ManHours { get; set; }
        public DateTime StartOfWork { get; set; } = DateTime.Now;

        public string DelayReason { get; set; }
        public string JobDescription { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = "The Man Power must be greater than zero.")]
        public long ManPower { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = "The Equipment No must be greater than zero.")]
        public long EquipmentNo { get; set; }
        [Range(1, double.MaxValue, ErrorMessage = "The Hours Delayed must be greater than zero.")]
        public double HoursDelayed { get; set; }
        public ApproveStatus Status { get; set; }

        public DepartmentBriefViewModel Department { get; set; } = new DepartmentBriefViewModel();

        public UnitBriefViewModel Unit { get; set; } = new UnitBriefViewModel();

        public ContractorBriefViewModel Contractor { get; set; } = new ContractorBriefViewModel();

        public ShiftDelayBriefViewModel ShiftDelay { get; set; } = new ShiftDelayBriefViewModel();

        public ReworkDelayBriefViewModel ReworkDelay { get; set; } = new ReworkDelayBriefViewModel();

        public PermitTypeBriefViewModel PermitType { get; set; } = new PermitTypeBriefViewModel();

        public ShiftBriefViewModel Shift { get; set; } = new ShiftBriefViewModel();

        public UserBriefViewModel Approver { get; set; } = new UserBriefViewModel(true, "The Approver field is required.");

        public UserBriefViewModel Foreman { get; set; } = new UserBriefViewModel(true, "The Foreman field is required.");
    }
}
