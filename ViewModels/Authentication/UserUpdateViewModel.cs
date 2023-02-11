//using Helpers.File;
//using Microsoft.AspNetCore.Http;
//using Models.Common.Interfaces;
//using System.ComponentModel.DataAnnotations;
//using ViewModels.Shared;

//namespace ViewModels.Authentication
//{
//    public class UserUpdateViewModel : BaseUpdateVM, IBaseCrudViewModel, IIdentitifier
//    {
//        [Required]
//        [EmailAddress]
//        public string Email { get; set; }
//        [Required]
//        [Display(Name = "Username")]
//        public string UserName { get; set; }
//        [Required]
//        [DataType(DataType.Password)]
//        public string Password { get; set; }
//        [Required]
//        [DataType(DataType.Password)]
//        [Display(Name = "Confirm Password")]
//        [Compare("Password", ErrorMessage = "Confirm Password Does not Match with password")]
//        public string ConfirmPassword { get; set; }

//        [Required]
//        [DataType(DataType.PhoneNumber)]
//        [Display(Name = "Phone Number")]
//        [RegularExpression(@"^[0-9]{7,15}$", ErrorMessage = "Phone no. must be a valid number")]
//        public string PhoneNumber { get; set; }
//    }
//}
