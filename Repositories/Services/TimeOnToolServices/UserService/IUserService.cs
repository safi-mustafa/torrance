using Pagination;
using ViewModels.Authentication;
using ViewModels.Authentication.Approver;

namespace Repositories.Services.TimeOnToolServices.UserService
{
    public interface IUserService 
    {
        Task<PaginatedResultModel<UserBriefViewModel>> GetUsers(UserSearchViewModel model);
    }
}
