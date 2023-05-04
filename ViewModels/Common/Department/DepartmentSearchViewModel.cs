using Enums;
using Pagination;
using ViewModels.Common.Unit;
using ViewModels.OverrideLogs;

namespace ViewModels.Common.Department
{
    public class DepartmentSearchViewModel : BaseSearchModel
    {
        public DepartmentSearchViewModel()
        {
            OrderByColumn = "Name";
            OrderDir = PaginationOrderCatalog.Asc;
        }
        public string Name { get; set; }

        public UnitBriefViewModel Unit { get; set; } = new();
        public bool IsSearchForm { get; set; }

        public bool ShowUnApproved { get; set; }
        public FilterLogType LogType { get; set; }
    }
}
