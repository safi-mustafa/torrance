using Pagination;

namespace ViewModels.TimeOnTools.OngoingWorkDelay
{
    public class OngoingWorkDelaySearchViewModel : BaseSearchModel
    {
        public OngoingWorkDelaySearchViewModel()
        {
            OrderByColumn = "Name";
            OrderDir = PaginationOrderCatalog.Asc;
        }
        public string Name { get; set; }
    }
}
