namespace ViewModels.Authentication
{
    public class UserDetailVM
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public List<UserRolesVM> Roles { get; set; } = new List<UserRolesVM>();
        public string ImageUrl { get; set; }
        public string Role { get; set; }
        public bool IsApproved { get; set; }
    }
}
