using System.ComponentModel.DataAnnotations;

namespace ViewModels.Authentication
{
    public class LoginVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        public string? DeviceId { get; set; }
    }

    public class LoginBriefVM
    {
        public string Pincode { get; set; }
        public string? DeviceId { get; set; }
    }
}
