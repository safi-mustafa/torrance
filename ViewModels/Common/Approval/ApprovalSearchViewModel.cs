using System;
using System.ComponentModel.DataAnnotations;
using Enums;
using Pagination;
using ViewModels.Authentication.User;
using ViewModels.Common.Company;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.WeldingRodRecord;

namespace ViewModels.Common
{
    public class ApprovalSearchViewModel : BaseSearchModel
    {
        public ApprovalSearchViewModel()
        {
        }
        public LogType? Type { get; set; }
        public ApprovalStatus? Status { get; set; } 

        [Display(Name = "Requestor")]
        public EmployeeBriefViewModel Employee { get; set; } = new(false,"");
        public CompanyBriefViewModel Company { get; set; } = new CompanyBriefViewModel(false, "");
        public DepartmentBriefViewModel Department { get; set; } = new DepartmentBriefViewModel(false);
        public UnitBriefViewModel Unit { get; set; } = new UnitBriefViewModel(false);
        public ApproverBriefViewModel Approver { get; set; } = new ApproverBriefViewModel(false);
    }
}

