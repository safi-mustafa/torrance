namespace Repositories.Shared.UserInfoServices
{
    public interface IUserInfoService
    {
        string LoggedInUserId();
        string LoggedInUserRole();
        string LoggedInEmployeeId();
        string LoggedInWebUserRole();
        string LoggedInUserDesignation();
        List<string> LoggedInUserRoles();
        string LoggedInUserImageUrl();
        string LoggedInUserEmail();
        string LoggedInUserFullName();
    }
}
