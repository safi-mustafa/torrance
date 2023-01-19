using Models.Common.Interfaces;
using Pagination;
using Repositories.Interfaces;
using ViewModels.Common.Unit;
using ViewModels.Shared;

namespace Repositories.Services.CommonServices.UnitService
{
    public interface IUnitService<CreateViewModel, UpdateViewModel, DetailViewModel> : IBaseCrud<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
    }
}
