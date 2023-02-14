using Pagination;

namespace ViewModels.WeldingRodRecord.Location
{
    public class LocationSearchViewModel : BaseSearchModel
    {
        public LocationSearchViewModel()
        {
            OrderByColumn = "Name";
            OrderDir = PaginationOrderCatalog.Asc;
        }
        public string Name { get; set; }
    }
}
