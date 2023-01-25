namespace Repositories.Shared.UserInfoServices
{
    public interface IUserInfoService
    {
        long LoggedInUserId();
        string LoggedInUserRole();
        string LoggedInWebUserRole();
        string LoggedInUserDesignation();
        List<string> LoggedInUserRoles();
        string LoggedInUserImageUrl();
        string LoggedInUserEmail();
        string LoggedInUserFullName();
    }
}
