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
            IDbContextTransaction transaction,
            string optionalUsernamePrefix = "");
        Task<bool> UpdateUser(SignUpModel model,
            IDbContextTransaction transaction);

        Task<PaginatedResultModel<T>> GetAll<T>(BaseSearchModel search);
        Task<bool> SendNotification(MailRequestViewModel mailRequest);
    }
}
