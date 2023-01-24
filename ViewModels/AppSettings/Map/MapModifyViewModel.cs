using Models.Common.Interfaces;
using ViewModels.Shared;

namespace ViewModels.AppSettings.Map
{
    public class MapModifyViewModel : BaseUpdateVM, IBaseCrudViewModel, IIdentitifier
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
