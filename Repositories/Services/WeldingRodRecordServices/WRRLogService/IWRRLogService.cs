using ClosedXML.Excel;
using Models.Common.Interfaces;
using Repositories.Interfaces;
using Repositories.Shared.Interfaces;
using ViewModels.Shared;
using ViewModels.TimeOnTools.TOTLog;
using ViewModels.WeldingRodRecord.WRRLog;

namespace Repositories.Services.AppSettingServices.WRRLogService
{
    public interface IWRRLogService<CreateViewModel, UpdateViewModel, DetailViewModel> : IBaseCrud<CreateViewModel, UpdateViewModel, DetailViewModel>,IBaseApprove
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        Task<bool> IsWRRLogEmailUnique(int id, string email);
        Task<XLWorkbook> DownloadExcel(WRRLogSearchViewModel searchModel);
    }
}
