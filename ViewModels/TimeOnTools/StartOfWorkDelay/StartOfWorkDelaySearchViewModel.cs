using Pagination;

namespace ViewModels.TimeOnTools.StartOfWorkDelay
{
    public class StartOfWorkDelaySearchViewModel : BaseSearchModel
    {
        public StartOfWorkDelaySearchViewModel()
        {
            OrderByColumn = "Name";
            OrderDir = PaginationOrderCatalog.Asc;
        }
        public string Name { get; set; }
    }
}
