using Pagination;

namespace ViewModels.TimeOnTools
{
    public class DelayTypeSearchViewModel : BaseSearchModel
    {
        public DelayTypeSearchViewModel()
        {
            OrderByColumn = "Order";
            OrderDir = PaginationOrderCatalog.Asc;
        }
        public string Name { get; set; }
    }
}
