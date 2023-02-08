﻿using Helpers.File;
using Helpers.ValidationAttributes;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using ViewModels.Common.Company;

namespace ViewModels.Authentication
{
    public class SignUpModel : IFileModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string EmployeeId { get; set; }
        [Required]
        [Display(Name = "Email is Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [Display(Name = "First Name is Required")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name is Required")]
        public string LastName { get; set; }
        public string Name { get { return $"{FirstName} {LastName}"; } }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password is Required")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password is Required")]
        [Compare("Password", ErrorMessage = "Confirm Password Does not Match with password")]
        public string ConfirmPassword { get; set; }
        public string AccessCode { get; set; }

        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [CustomMobileNumberValidator(ErrorMessage = "Phone no. must be a valid number")]
        public string PhoneNumber { get; set; }
        public string Action { get; set; }
        public string Role { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? File { get; set; }
        public bool IsApproved { get; set; } = true;
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
