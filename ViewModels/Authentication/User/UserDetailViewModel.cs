using Enums;
using Helpers.Extensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ViewModels.Common.Company;
using ViewModels.Common.Contractor;
using ViewModels.Shared;

namespace ViewModels.Authentication.User
{
    public class UserDetailViewModel : BaseCrudViewModel
    {
        public long Id { get; set; }
        public string Email { get; set; }
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        public List<UserRolesVM> Roles { get; set; } = new List<UserRolesVM>();
        public string Role { get; set; }
        public bool IsApproved { get; set; }

        public string AccessCode { get; set; }
        public string FormattedAccessCode { get => AccessCode?.DecodeFrom64(); }

        public ContractorBriefViewModel Contractor { get; set; } = new ContractorBriefViewModel();
        public CompanyBriefViewModel Company { get; set; } = new CompanyBriefViewModel();
    }
}
