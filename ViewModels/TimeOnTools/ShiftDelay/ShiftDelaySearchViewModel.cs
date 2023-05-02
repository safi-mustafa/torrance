using Pagination;

namespace ViewModels.TimeOnTools.ShiftDelay
{
    public class ShiftDelaySearchViewModel : BaseSearchModel
    {
        public ShiftDelaySearchViewModel()
        {
            OrderByColumn = "Name";
            OrderDir = PaginationOrderCatalog.Asc;
        }
        public string Name { get; set; }
    }
}
