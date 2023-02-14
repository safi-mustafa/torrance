using Pagination;

namespace ViewModels.TimeOnTools.PermitType
{
    public class PermitTypeSearchViewModel : BaseSearchModel
    {
        public PermitTypeSearchViewModel()
        {
            OrderByColumn = "Name";
            OrderDir = PaginationOrderCatalog.Asc;
        }
        public string Name { get; set; }
    }
}
