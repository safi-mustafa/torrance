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
        Task<IRepositoryResponse> CreateNotificationsForLogCreation(INotificationMetaViewModel meta);

        Task<IRepositoryResponse> CreateNotificationsForLogUpdation(INotificationMetaViewModel meta);
        Task<IRepositoryResponse> CreateNotificationsForLogApproverAssignment(INotificationMetaViewModel meta);

        Task<IRepositoryResponse> CreateNotificationsForLogAfterProcessing(INotificationMetaViewModel meta, NotificationEventTypeCatalog eventType);
        Task<IRepositoryResponse> Create(NotificationViewModel model);
        //Task<IRepositoryResponse> CreateLogNotificationAfterProcessing(NotificationViewModel model, long approverId);
    }
}
