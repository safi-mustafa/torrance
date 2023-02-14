using Pagination;

namespace ViewModels.TimeOnTools.Shift
{
    public class ShiftSearchViewModel : BaseSearchModel
    {
        public ShiftSearchViewModel()
        {
            OrderByColumn = "Name";
            OrderDir = PaginationOrderCatalog.Asc;
        }
        public string Name { get; set; }
    }
}
