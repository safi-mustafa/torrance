using Repositories.Interfaces;
using ViewModels.TomeOnTools.TOTLog;

namespace Repositories.Services.TimeOnToolServices.TOTLogService
{
    public interface ITOTLogService : IBaseCrud<TOTLogModifyViewModel, TOTLogModifyViewModel, TOTLogDetailViewModel>
    {
    }
}
