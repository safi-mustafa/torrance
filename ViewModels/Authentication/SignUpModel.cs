using Enums;
using Helpers.File;
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
        public bool IsExcelSheet { get; set; }
        public string EmployeeId { get; set; }
        [Required]
        [Display(Name = "Email is Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Full Name is Required")]
        public string FullName { get; set; }
        public string Name { get { return $"{FullName}"; } }
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
        public ActiveStatus ActiveStatus { get; set; }
        public string Action { get; set; }
        public string Role { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? File { get; set; }
        public CompanyBriefViewModel Company { get; set; } = new CompanyBriefViewModel();
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
        public bool ChangePassword { get; set; }

        public bool CanAddLogs { get; set; }
        public bool DisableNotifications { get; set; }
    }
}
