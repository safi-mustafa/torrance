using Repositories.Interfaces;
using ViewModels.TomeOnTools.Shift;

namespace Repositories.Services.TimeOnToolServices.ShiftService
{
    public interface IShiftService : IBaseCrud<ShiftModifyViewModel, ShiftModifyViewModel, ShiftDetailViewModel>
    {
    }
}
