using Pagination;

namespace ViewModels.Common.Unit
{
    public class UnitSearchViewModel : BaseSearchModel
    {
        public UnitSearchViewModel()
        {
            OrderByColumn = "Name";
            OrderDir = PaginationOrderCatalog.Asc;
        }
        public string Name { get; set; }
    }
}
