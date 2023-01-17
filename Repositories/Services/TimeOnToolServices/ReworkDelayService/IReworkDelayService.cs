using Repositories.Interfaces;
using ViewModels.TomeOnTools.ReworkDelay;

namespace Repositories.Services.TimeOnToolServices.ReworkService
{
    public interface IReworkDelayService : IBaseCrud<ReworkDelayModifyViewModel, ReworkDelayModifyViewModel, ReworkDelayDetailViewModel>
    {
    }
}
