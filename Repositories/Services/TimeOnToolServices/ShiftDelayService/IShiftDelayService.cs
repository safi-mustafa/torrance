using Repositories.Interfaces;
using ViewModels.TomeOnTools.ShiftDelay;

namespace Repositories.Services.TimeOnToolServices.ShiftDelayService
{
    public interface IShiftDelayService : IBaseCrud<ShiftDelayModifyViewModel, ShiftDelayModifyViewModel, ShiftDelayDetailViewModel>
    {
    }
}
