using Pagination;

namespace ViewModels.OverrideLogs
{
    public class ReasonForRequestSearchViewModel : BaseSearchModel
    {
        public ReasonForRequestSearchViewModel()
        {
            OrderByColumn = "Name";
            OrderDir = PaginationOrderCatalog.Asc;
        }
        public string Name { get; set; }
    }
}
