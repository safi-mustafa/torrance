using Enums;
using ViewModels.Notification;

namespace Repositories.Shared.NotificationServices
{
    public interface INotificationService
    {
        Task<bool> AddNotificationAsync(NotificationViewModel model);
    }
}
