using ClosedXML.Excel;
using Models.Common.Interfaces;
using Repositories.Interfaces;
using Repositories.Shared.Interfaces;
using ViewModels.Shared;
using ViewModels;
using Enums;
using Centangle.Common.ResponseHelpers.Models;
using ViewModels.FCO.FCOLog;

namespace Repositories.Services.AppSettingServices
{
    public interface IFCOLogService<CreateViewModel, UpdateViewModel, DetailViewModel> : IBaseCrud<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        Task<bool> IsFCOLogEmailUnique(int id, string email);
        Task<XLWorkbook> DownloadExcel(FCOLogSearchViewModel searchModel);
        Task<IRepositoryResponse> SetApproveStatus(FCOLogApproveViewModel model);
        Task<IRepositoryResponse> GetFCOComments(long fcoId);

        long GetMaxSectionCount();
    }
}
