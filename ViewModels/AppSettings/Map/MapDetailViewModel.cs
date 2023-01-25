using ViewModels.Shared;

namespace ViewModels.AppSettings.Map
{
    public class MapDetailViewModel : BaseCrudViewModel
    {
        public long Id { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
