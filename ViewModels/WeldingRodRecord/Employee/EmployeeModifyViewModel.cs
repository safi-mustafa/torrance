using ViewModels.Shared;
using Models.Common.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Helpers.ValidationAttributes;
using ViewModels.Authentication;
using ViewModels.Common.Contractor;
using Enums;
using ViewModels.Common.Company;

namespace ViewModels.WeldingRodRecord.Employee
{
    public class EmployeeModifyViewModel : BaseUpdateVM, IBaseCrudViewModel, IIdentitifier
    {
        [MaxLength(200)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [MaxLength(200)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Access Code")]
        [RegularExpression(@"^(\d{4})$", ErrorMessage = "Access Code must be of 4-digits.")]
        [Remote(action: "ValidateEmployeeId", controller: "Employee", AdditionalFields = "Id,EmployeeId", ErrorMessage = "Access Code already in use.")]
        public string EmployeeId { get; set; }
        [DataType(DataType.PhoneNumber)]
        //[CustomMobileNumberValidator(ErrorMessage = "Telephone No. must be a valid number")]
        public long? Telephone { get; set; }
        [EmailAddress]
        [Remote(action: "ValidateEmployeeEmail", controller: "Employee", AdditionalFields = "Id,Email", ErrorMessage = "Employee Email already in use.")]
        public string Email { get; set; }
        public string? Address { get; set; }
        public string? Action { get; set; }
        public string? Role { get; set; }
        public long UserId { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
      //  public StateBriefViewModel State { get; set; } = new StateBriefViewModel();
        [DisplayName("Zip Code")]
        [StringLength(5, ErrorMessage = "Zip must be a 5 digit code", MinimumLength = 5)]
        public string? ZipCode { get; set; }
        [DisplayName("Social Security")]
        public string? SocialSecurity { get; set; }
        [DisplayName("Driver's License")]
        public string? DriverLicense { get; set; }
        [DisplayName("Bank Account")]
        public long? BankAccount { get; set; }
        [DisplayName("Routing Number")]
        public long? RoutingNumber { get; set; }
        [DisplayName("Emergency Contact Name")]
        public string? EmergencyContactName { get; set; }
        [DisplayName("Emergency Contact Number")]
        public long? EmergencyContactNumber { get; set; }
        [DisplayName("Date of Hire")]
        public DateTime DateOfHire { get; set; } = DateTime.Now;
        private DateTime _terminationDate = DateTime.Now;
        [DisplayName("Termination Date")]
        public DateTime TerminationDate
        {
            get => _terminationDate;
            set
            {
                if (value == DateTime.MinValue)
                    _terminationDate = DateTime.Now;
                else
                    _terminationDate = value;
            }
        }

        public ContractorBriefViewModel Contractor { get; set; } = new ContractorBriefViewModel();
        public CompanyBriefViewModel Company { get; set; } = new CompanyBriefViewModel();

        public ApproverStatus IsApprover { get; set; }

    }
}
