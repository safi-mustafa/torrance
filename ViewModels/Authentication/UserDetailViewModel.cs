using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using ViewModels.Shared;

namespace ViewModels.Authentication
{
    public class UserDetailViewModel : BaseCrudViewModel
    {
        public long Id { get; set; }
        public string Email { get; set; }
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        public List<UserRolesVM> Roles { get; set; } = new List<UserRolesVM>();
        public string Role { get; set; }
        public bool IsApproved { get; set; }
    }
}
