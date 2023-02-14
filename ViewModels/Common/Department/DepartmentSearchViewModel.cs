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
    }
}
