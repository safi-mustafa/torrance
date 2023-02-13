using Models.Common.Interfaces;
using Repositories.Services.CommonServices.UserService;
using ViewModels.Shared;

namespace Repositories.Services.AppSettingServices.ForemanService
{
    public interface IForemanService<CreateViewModel, UpdateViewModel, DetailViewModel> : IUserService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
    }
}
