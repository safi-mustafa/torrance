using Pagination;

namespace ViewModels
{
    public class FCOReasonSearchViewModel : BaseSearchModel
    {
        public FCOReasonSearchViewModel()
        {
            OrderByColumn = "Name";
            OrderDir = PaginationOrderCatalog.Asc;
        }
        public string Name { get; set; }
    }
}
