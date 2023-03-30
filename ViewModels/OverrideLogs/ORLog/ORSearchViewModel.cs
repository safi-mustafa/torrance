using Enums;
using Pagination;
using System.ComponentModel.DataAnnotations;
using ViewModels.Authentication;
using ViewModels.Authentication.User;
using ViewModels.Common.Company;
using ViewModels.Common.Unit;
using ViewModels.WeldingRodRecord;

namespace ViewModels.OverrideLogs.ORLog
{
    public class ORLogSearchViewModel : BaseSearchModel
    {
        public EmployeeBriefViewModel Requester { get; set; } = new EmployeeBriefViewModel(false);
        public ApproverBriefViewModel Approver { get; set; } = new ApproverBriefViewModel(false);
        public UnitBriefViewModel Unit { get; set; } = new UnitBriefViewModel(false);
        public OverrideTypeCatalog? OverrideType { get; set; }
        public CompanyBriefViewModel Company { get; set; } = new CompanyBriefViewModel(false, "");

        public StatusSearchEnum? Status { get; set; } = null;
        [Display(Name = "Status Is Not")]
        public Status? StatusNot { get; set; } = null;
    }

    public class ORLogAPISearchViewModel : BaseSearchModel
    {
        public long RequesterId { get; set; }
        public long ApproverId { get; set; }
        public long UnitId { get; set; }
        public long OverrideTypeId { get; set; }
        public long CompanyId { get; set; }
    }
}
