using Repositories.Interfaces;
using ViewModels.WeldingRodRecord.WeldMethod;

namespace Repositories.Services.WeldRodRecordServices.WeldMethodService
{
    public interface IWeldMethodService : IBaseCrud<WeldMethodModifyViewModel, WeldMethodModifyViewModel, WeldMethodDetailViewModel>
    {
    }
}
