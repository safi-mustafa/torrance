using Pagination;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.WeldingRodRecord.Location;
using ViewModels.WeldingRodRecord.RodType;
using ViewModels.WeldingRodRecord.WeldMethod;

namespace ViewModels.WeldingRodRecord.WRRLog
{
    public class WRRLogSearchViewModel : BaseSearchModel
    {
        public string Email { get; set; }
        public DepartmentBriefViewModel Department { get; set; } = new DepartmentBriefViewModel();

        public EmployeeBriefVM Employee { get; set; } = new EmployeeBriefVM();

        public UnitBriefViewModel Unit { get; set; } = new UnitBriefViewModel();

        public RodTypeBriefViewModel RodType { get; set; } = new RodTypeBriefViewModel();

        public WeldMethodBriefViewModel WeldMethod { get; set; } = new WeldMethodBriefViewModel();

        public LocationBriefViewModel Location { get; set; } = new LocationBriefViewModel();
    }
}
