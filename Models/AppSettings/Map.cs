using Helpers.Models.Shared;

namespace Models.AppSettings
{
    public class Map : BaseDBModel
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
