using Enums;
using Models.Common.Interfaces;
using Repositories.Interfaces;
using ViewModels.Notification;
using ViewModels.Shared;

namespace Repositories.Shared.NotificationServices
{
    public interface INotificationService<CreateViewModel, UpdateViewModel, DetailViewModel> : IBaseCrud<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
    }
}
