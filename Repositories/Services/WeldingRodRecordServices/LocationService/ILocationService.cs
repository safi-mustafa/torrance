using Pagination;
using Repositories.Interfaces;
using ViewModels.TomeOnTools.PermitType;
using ViewModels.TomeOnTools.ReworkDelay;
using ViewModels.TomeOnTools.Shift;
using ViewModels.TomeOnTools.ShiftDelay;
using ViewModels.TomeOnTools.SOW;
using ViewModels.WeldingRodRecord.Location;

namespace Repositories.Services.WeldRodRecordServices.LocationService
{
    public interface ILocationService : IBaseCrud<LocationModifyViewModel, LocationModifyViewModel, LocationDetailViewModel>
    {
    }
}
