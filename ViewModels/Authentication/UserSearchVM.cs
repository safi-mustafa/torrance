using Pagination;

namespace ViewModels.Authentication
{
    public class UserSearchViewModel : BaseSearchModel
    {
        public string Type { get; set; }
        public string Email { get; set; }
        public List<UserRolesVM> Roles { get; set; } = new List<UserRolesVM>();
    }
}
