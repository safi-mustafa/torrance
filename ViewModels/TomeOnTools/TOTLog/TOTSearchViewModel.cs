using Pagination;
using ViewModels.Authentication;
using ViewModels.Common.Contractor;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.TomeOnTools.PermitType;
using ViewModels.TomeOnTools.ReworkDelay;
using ViewModels.TomeOnTools.Shift;
using ViewModels.TomeOnTools.ShiftDelay;

namespace ViewModels.TomeOnTools.TOTLog
{
    public class TOTLogSearchViewModel : BaseSearchModel
    {
        public long EquipmentNo { get; set; }
        public DepartmentBriefViewModel Department { get; set; } = new DepartmentBriefViewModel();

        public UnitBriefViewModel Unit { get; set; } = new UnitBriefViewModel();

        public ContractorBriefViewModel Contractor { get; set; } = new ContractorBriefViewModel();

        public ShiftDelayBriefViewModel ShiftDelay { get; set; } = new ShiftDelayBriefViewModel();

        public ReworkDelayBriefViewModel ReworkDelay { get; set; } = new ReworkDelayBriefViewModel();

        public PermitTypeBriefViewModel PermitType { get; set; } = new PermitTypeBriefViewModel();

        public ShiftBriefViewModel Shift { get; set; } = new ShiftBriefViewModel();

        public UserBriefViewModel Approver { get; set; } = new UserBriefViewModel();

        public UserBriefViewModel Foreman { get; set; } = new UserBriefViewModel();
    }
}
