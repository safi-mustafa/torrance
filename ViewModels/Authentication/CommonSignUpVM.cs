﻿using Helpers.File;
using Helpers.ValidationAttributes;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.Authentication
{
    public class CommonSignUpVM 
    {
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Confirm Password Does not Match with password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [CustomMobileNumberValidator(ErrorMessage = "Phone no. must be a valid number")]

        public string PhoneNumber { get; set; }
    }
}
