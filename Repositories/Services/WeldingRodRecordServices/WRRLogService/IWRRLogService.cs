using Repositories.Interfaces;
using ViewModels.WeldingRodRecord.WRRLog;

namespace Repositories.Services.WeldRodRecordServices.WRRLogService
{
    public interface IWRRLogService : IBaseCrud<WRRLogModifyViewModel, WRRLogModifyViewModel, WRRLogDetailViewModel>
    {
    }
}
