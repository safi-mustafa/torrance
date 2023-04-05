using Enums;
using Microsoft.EntityFrameworkCore.Storage;
using Pagination;
using ViewModels.Authentication;
using ViewModels.Notification;

namespace Repositories.Shared.AuthenticationService
{
    public interface IIdentityService
    {
        Task<long> CreateUser(SignUpModel model,
            string optionalUsernamePrefix = "");
        Task<bool> UpdateUser(SignUpModel model);

        Task<PaginatedResultModel<T>> GetAll<T>(BaseSearchModel search);
        Task<bool> SendNotification(MailRequestViewModel mailRequest);
    }
}
