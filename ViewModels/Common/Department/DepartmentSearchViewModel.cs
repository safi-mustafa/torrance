using Pagination;

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
    }
}
