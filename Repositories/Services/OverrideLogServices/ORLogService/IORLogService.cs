using Centangle.Common.ResponseHelpers.Models;
using Models.Common.Interfaces;
using Pagination;
using Repositories.Interfaces;
using Repositories.Shared.Interfaces;
using ViewModels.Shared;

namespace Repositories.Services.OverrideLogServices.ORLogService
{
    public interface IORLogService<CreateViewModel, UpdateViewModel, DetailViewModel> : IBaseCrud<CreateViewModel, UpdateViewModel, DetailViewModel>, IBaseApprove
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        Task<IRepositoryResponse> GetOverrideTypes<BaseBriefVM>(IBaseSearchModel search);
    }
}
