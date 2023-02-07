using Centangle.Common.ResponseHelpers.Models;
using Models.Common.Interfaces;
using Pagination;
using Repositories.Interfaces;
using Repositories.Shared.Interfaces;
using ViewModels.Shared;

namespace Repositories.Services.TimeOnToolServices.TOTLogService
{
    public interface ITOTLogService<CreateViewModel, UpdateViewModel, DetailViewModel> : IBaseCrud<CreateViewModel, UpdateViewModel, DetailViewModel>, IBaseApprove
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        Task<IRepositoryResponse> GetTWRNumericValues<M>(IBaseSearchModel search);
        Task<IRepositoryResponse> GetTWRAphabeticValues<M>(IBaseSearchModel search);
    }
}
