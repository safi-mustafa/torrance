using Enums;
using Microsoft.EntityFrameworkCore.Storage;
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
    }
}
