using Enums;
using Microsoft.AspNetCore.Mvc;
using Models.Common.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ViewModels.Common.Company;
using ViewModels.Common.Contractor;
using ViewModels.Common.Unit;
using ViewModels.MultiSelectInterfaces;
using ViewModels.Shared;

namespace ViewModels.Authentication.User
{
    public class UserUpdateViewModel : BaseUpdateVM, IBaseCrudViewModel, IIdentitifier
    {

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        //[Required]
        //[Display(Name = "Username")]
        //public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Confirm Password Does not Match with password")]
        public string? ConfirmPassword { get; set; }

        [DisplayName("Access Code")]
        [RegularExpression(@"^(\d{4})$", ErrorMessage = "Access Code must be of 4-digits.")]
        [Remote(action: "ValidateAccessCode", controller: "User", AdditionalFields = "Id,AccessCode", ErrorMessage = "Access Code already in use.")]
        public string AccessCode { get; set; }

        //[Required]
        //[Display(Name = "Confirm Access Code")]
        //[Compare("AccessCode", ErrorMessage = "Confirm Access Code Does not Match with Access Code")]
        //public string ConfirmAccessCode { get; set; }

        public RolesCatalog? Role { get; set; }
        public bool IsExcelSheet { get; set; }
        public ContractorBriefViewModel Contractor { get; set; } = new ContractorBriefViewModel();
        public CompanyBriefViewModel Company { get; set; } = new CompanyBriefViewModel();

        public bool ChangePassword { get; set; }

        [DisplayName("Can Add Logs?")]

        public bool CanAddLogs { get; set; }
    }
}
