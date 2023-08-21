using ClosedXML.Excel;
using Models.Common.Interfaces;
using Repositories.Interfaces;
using Repositories.Shared.Interfaces;
using ViewModels.Shared;
using ViewModels;
using Enums;
using Centangle.Common.ResponseHelpers.Models;

namespace Repositories.Services.AppSettingServices
{
    public interface IFCOLogService<CreateViewModel, UpdateViewModel, DetailViewModel> : IBaseCrud<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        Task<bool> IsFCOLogEmailUnique(int id, string email);
        Task<XLWorkbook> DownloadExcel(FCOLogSearchViewModel searchModel);
        Task<IRepositoryResponse> SetApproveStatus(long id, Status status, bool isUnauthenticatedApproval = false, long approverId = 0, Guid notificationId = new Guid(), string comment = "", ApproverType approverType = 0);
        Task<IRepositoryResponse> GetFCOComments(long fcoId);
        long GetMaxSectionCount();
    }
}
