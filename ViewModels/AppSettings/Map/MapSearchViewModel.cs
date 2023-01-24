using Pagination;

namespace ViewModels.AppSettings.Map
{
    public class MapSearchViewModel : BaseSearchModel
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
