using Repositories.Interfaces;
using ViewModels.WeldingRodRecord.RodType;

namespace Repositories.Services.WeldRodRecordServices.RodTypeService
{
    public interface IRodTypeService : IBaseCrud<RodTypeModifyViewModel, RodTypeModifyViewModel, RodTypeDetailViewModel>
    {
    }
}
