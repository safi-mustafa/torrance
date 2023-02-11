using ViewModels.Shared;
using Pagination;
using Select2.Model;
using Enums;
using System.ComponentModel;
using ViewModels.CRUD;
using ViewModels.Authentication;
using ViewModels.Common.Contractor;
using ViewModels.Common.Company;
using ViewModels.Authentication.User;

namespace ViewModels.AppSettings.CompanyManager
{
    public class CompanyManagerDetailViewModel : UserDetailViewModel, ISelect2Data
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
        
    }

}