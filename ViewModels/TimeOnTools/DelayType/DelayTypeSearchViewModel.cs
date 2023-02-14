using Pagination;

namespace ViewModels.TimeOnTools
{
    public class DelayTypeSearchViewModel : BaseSearchModel
    {
        public DelayTypeSearchViewModel()
        {
            OrderByColumn = "Name";
            OrderDir = PaginationOrderCatalog.Asc;
        }
        public string Name { get; set; }
    }
}
