using Enums;
using Helpers.Extensions;
using Select2.Model;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ViewModels.Common.Company;
using ViewModels.Common.Contractor;
using ViewModels.Shared;

namespace ViewModels.Authentication.User
{
    public class UserDetailViewModel : BaseCrudViewModel, ISelect2Data
    {
        public long? Id { get; set; }
        public string Name
        {
            get
            {
                return FullName;
            }
            set { }
        }
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

        public bool ChangePassword { get; set; }

        [Display(Name = "Can Add Logs?")]
        public bool CanAddLogs { get; set; }

        public string FormattedCanAddLogs
        {
            get
            {
                return CanAddLogs ? "Yes" : "No";
            }
        }
        [Display(Name = "Disable Notifications?")]
        public bool DisableNotifications { get; set; }
        public string FormattedDisableNotifications
        {
            get
            {
                return DisableNotifications ? "Yes" : "No";
            }
        }
    }
}
