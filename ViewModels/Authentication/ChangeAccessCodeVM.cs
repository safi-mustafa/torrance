using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ViewModels.Shared;
using Enums;

namespace ViewModels.Authentication
{
    public class ChangeAccessCodeVM 
    {
        [DisplayName("New Access Code")]
        [RegularExpression(@"^(\d{4})$", ErrorMessage = "Access Code must be of 4-digits.")]
        [Remote(action: "ValidateAccessCode", controller: "Approver", AdditionalFields = "Id,AccessCode", ErrorMessage = "Access Code already in use.")]
        public string AccessCode { get; set; }
        public long Id { get; set; }
        public string CurrentAccessCode { get; set; }


        [Required]
        [DisplayName("Confirm Access Code")]
        [Compare("AccessCode", ErrorMessage = "Confirm Access Code Does not Match.")]
        public string ConfirmAccessCode { get; set; }

    }
}
