using System;
using Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Models.Common.Interfaces;
using ViewModels.Common.Company;
using ViewModels.Common.Contractor;
using ViewModels.Shared;

namespace ViewModels.WeldingRodRecord.Employee
{
    public class EmployeeCreateViewModel : BaseCreateVM, IBaseCrudViewModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DisplayName("Access Code")]
        [RegularExpression(@"^(\d{4})$", ErrorMessage = "Access Code must be of 4-digits.")]
        [Remote(action: "ValidateAccessCode", controller: "User", AdditionalFields = "Id,AccessCode", ErrorMessage = "Access Code already in use.")]
        public string AccessCode { get; set; }
        public ContractorBriefViewModel Contractor { get; set; } = new ContractorBriefViewModel();
        public CompanyBriefViewModel Company { get; set; } = new CompanyBriefViewModel();
    }
}

