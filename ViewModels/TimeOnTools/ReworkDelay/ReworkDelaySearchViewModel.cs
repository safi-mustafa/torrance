using Pagination;

namespace ViewModels.TimeOnTools.ReworkDelay
{
    public class ReworkDelaySearchViewModel : BaseSearchModel
    {
        public ReworkDelaySearchViewModel()
        {
            OrderByColumn = "Name";
            OrderDir = PaginationOrderCatalog.Asc;
        }
        public string Name { get; set; }
    }
}
