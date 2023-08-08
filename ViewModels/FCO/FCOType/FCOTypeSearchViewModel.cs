using Pagination;

namespace ViewModels
{
    public class FCOTypeSearchViewModel : BaseSearchModel
    {
        public FCOTypeSearchViewModel()
        {
            OrderByColumn = "Name";
            OrderDir = PaginationOrderCatalog.Asc;
        }
        public string Name { get; set; }
    }
}
