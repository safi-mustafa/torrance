using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ViewModels.WeldingRodRecord.Employee
{
    public class EmployeeExcelViewModel
    {
        public bool IsExcelSheet { get; set; } = true;
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
        public string Password { get; set; }

        [DisplayName("Access Code")]
        [RegularExpression(@"^(\d{4})$", ErrorMessage = "Access Code must be of 4-digits.")]
        [Remote(action: "ValidateAccessCode", controller: "User", AdditionalFields = "Id,AccessCode", ErrorMessage = "Access Code already in use.")]
        public string AccessCode { get; set; }

        public string CompanyName { get; set; }

    }
}

