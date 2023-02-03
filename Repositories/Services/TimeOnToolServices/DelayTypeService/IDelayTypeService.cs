using Models.Common.Interfaces;
using Repositories.Interfaces;
using ViewModels.Shared;
using ViewModels.TimeOnTools;

namespace Repositories.Services.TimeOnToolServices.DelayTypeService
{
    public interface IDelayTypeService<CreateViewModel, UpdateViewModel, DetailViewModel> : IBaseCrud<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
    }
}
