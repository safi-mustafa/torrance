using Enums;
using Microsoft.EntityFrameworkCore.Storage;
using Pagination;
using ViewModels.Authentication;

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
    }
}
