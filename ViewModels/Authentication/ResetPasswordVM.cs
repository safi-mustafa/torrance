using System.ComponentModel.DataAnnotations;

namespace ViewModels.Authentication
{
    public class ResetPasswordVM
    {

        public string Email { get; set; }

        public string? Token { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password is Required")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password is Required")]
        [Compare("Password", ErrorMessage = "Confirm Password Does not Match with password")]
        public string ConfirmPassword { get; set; }

    }
}
