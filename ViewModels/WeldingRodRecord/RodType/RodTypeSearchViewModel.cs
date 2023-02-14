using Pagination;

namespace ViewModels.WeldingRodRecord.RodType
{
    public class RodTypeSearchViewModel : BaseSearchModel
    {
        public RodTypeSearchViewModel()
        {
            OrderByColumn = "Name";
            OrderDir = PaginationOrderCatalog.Asc;
        }
        public string Name { get; set; }
    }
}
