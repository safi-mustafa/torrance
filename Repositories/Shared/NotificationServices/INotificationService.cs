using Centangle.Common.ResponseHelpers.Models;
using Enums;
using Models.Common.Interfaces;
using Repositories.Interfaces;
using ViewModels.Notification;
using ViewModels.Shared;

namespace Repositories.Shared.NotificationServices
{
    public interface INotificationService : IBaseSearch
    {
        Task<IRepositoryResponse> CreateLogNotification(NotificationViewModel model);
        Task<IRepositoryResponse> Create(NotificationViewModel model);
    }
}
