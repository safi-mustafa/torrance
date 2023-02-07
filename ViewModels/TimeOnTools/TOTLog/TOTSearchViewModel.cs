using Enums;
using Pagination;
using System.ComponentModel.DataAnnotations;
using ViewModels.Authentication;
using ViewModels.Common.Unit;
using ViewModels.TimeOnTools.PermitType;
using ViewModels.TimeOnTools.Shift;
using ViewModels.WeldingRodRecord;

namespace ViewModels.TimeOnTools.TOTLog
{
    public class TOTLogSearchViewModel : BaseSearchModel
    {
        [Display(Name = "Equipment No")]
        public long? EquipmentNo { get; set; }
        public StatusSearchEnum? Status { get; set; } = null;
        [Display(Name = "Status Is Not")]
        public Status? StatusNot { get; set; } = null;

        public UnitBriefViewModel Unit { get; set; } = new UnitBriefViewModel();
        public ShiftBriefViewModel Shift { get; set; } = new ShiftBriefViewModel();
        public DelayTypeBriefViewModel DelayType { get; set; } = new DelayTypeBriefViewModel();
        public PermitTypeBriefViewModel PermitType { get; set; } = new PermitTypeBriefViewModel();
        public EmployeeBriefViewModel Requester { get; set; } = new EmployeeBriefViewModel();
        public ApproverBriefViewModel Approver { get; set; } = new ApproverBriefViewModel();
    }

    public class TOTLogAPISearchViewModel : BaseSearchModel
    {
        public long? EquipmentNo { get; set; }
        public long? UnitId { get; set; }
        public long? ShiftId { get; set; }
        public long? DelayTypeId { get; set; }
        public long? PermitTypeId { get; set; }
        public long? RequesterId { get; set; }
        public long? ApproverId { get; set; }

    }
}
