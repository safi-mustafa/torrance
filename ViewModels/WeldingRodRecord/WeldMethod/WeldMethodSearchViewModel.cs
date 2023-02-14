using Pagination;

namespace ViewModels.WeldingRodRecord.WeldMethod
{
    public class WeldMethodSearchViewModel : BaseSearchModel
    {
        public WeldMethodSearchViewModel()
        {
            OrderByColumn = "Name";
            OrderDir = PaginationOrderCatalog.Asc;
        }
        public string Name { get; set; }
    }
}
