using Pagination;
using Repositories.Interfaces;
using ViewModels.Common.Unit;

namespace Repositories.Services.CommonServices.UnitService
{
    public interface IUnitService : IBaseCrud<UnitModifyViewModel, UnitModifyViewModel, UnitDetailViewModel>
    {
    }
}
