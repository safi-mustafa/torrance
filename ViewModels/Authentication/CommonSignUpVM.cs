﻿using Helpers.File;
using Helpers.ValidationAttributes;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.Authentication
{
    public class CommonSignUpVM : IFileModel
    {
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
      //  public string Name { get { return $"{FirstName} {LastName}"; } }
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
        //[Remote("IsUsernameUnique", "Account", ErrorMessage = "User Name already in use")] 
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [CustomMobileNumberValidator(ErrorMessage = "Phone no. must be a valid number")]

        public string PhoneNumber { get; set; }
        public string ImageUrl { get; set; }
        //public string DisplayImageUrl
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(ImageUrl))
        //        {
        //            return "/Storage/Default/default.png";
        //        }
        //        return ImageUrl;
        //    }
        //}
        public IFormFile File { get; set; }

        public string GetBaseFolder()
        {
            var ext = Path.GetExtension(File.FileName);
            if (ext == ".jpg" || ext == ".jpeg" || ext == ".png")
            {
                return "Images";
            }
            if (ext == ".mp4")
            {
                return "Videos";
            }
            if (ext == ".pdf" || ext == ".docx" || ext == ".xlsx" || ext == ".txt")
            {
                return "Documents";
            }
            return "Others";
        }
    }
}
