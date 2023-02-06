using Pagination;
using ViewModels.Authentication;

namespace Repositories.Services.TimeOnToolServices.UserService
{
    public interface IUserService 
    {
        Task<PaginatedResultModel<UserBriefViewModel>> GetUsers(UserSearchViewModel model);
    }
}
