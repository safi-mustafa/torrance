using Centangle.Common.ResponseHelpers.Models;
using Models.Common.Interfaces;
using Repositories.Interfaces;
using ViewModels.Authentication;
using ViewModels.Shared;

namespace Repositories.Services.CommonServices.UserService
{
    public interface IUserService<CreateViewModel, UpdateViewModel, DetailViewModel> : IBaseCrud<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        Task<bool> IsAccessCodeUnique(long id, string accessCode);
        Task<bool> IsEmailUnique(long id, string email);
        Task<IRepositoryResponse> ResetAccessCode(ChangeAccessCodeVM model);
    }
}
