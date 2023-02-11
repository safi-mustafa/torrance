using Pagination;
using ViewModels.Common.Unit;

namespace ViewModels.Authentication.User
{
    public class UserSearchViewModel : BaseSearchModel
    {
        public string? FullName { get; set; }
        public string? Type { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public List<UserRolesVM> Roles { get; set; } = new List<UserRolesVM>();
    }
}
