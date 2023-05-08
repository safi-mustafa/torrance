using Enums;
using Pagination;
using System.ComponentModel.DataAnnotations;
using ViewModels.Authentication;
using ViewModels.Authentication.User;
using ViewModels.Common.Company;
using ViewModels.Common.Contractor;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.TimeOnTools.Shift;
using ViewModels.WeldingRodRecord;

namespace ViewModels.OverrideLogs.ORLog
{
    public class ORLogSearchViewModel : BaseSearchModel
    {
        public EmployeeBriefViewModel Requestor { get; set; } = new EmployeeBriefViewModel(false);
        public ApproverBriefViewModel Approver { get; set; } = new ApproverBriefViewModel(false);
        public UnitBriefViewModel Unit { get; set; } = new UnitBriefViewModel(false);
        public OverrideTypeCatalog? OverrideType { get; set; }
        public CompanyBriefViewModel Company { get; set; } = new CompanyBriefViewModel(false, "");
        public DepartmentBriefViewModel Department { get; set; } = new DepartmentBriefViewModel(false);
        public ShiftBriefViewModel Shift { get; set; } = new ShiftBriefViewModel(false);
        public StatusSearchEnum? Status { get; set; } = null;
        [Display(Name = "Status Is Not")]
        public Status? StatusNot { get; set; } = null;

        public bool IsExcelDownload { get; set; } = false;
    }

    public class ORLogAPISearchViewModel : BaseSearchModel
    {
        public long RequestorId { get; set; }
        public long ApproverId { get; set; }
        public long UnitId { get; set; }
        public long OverrideTypeId { get; set; }
        public long CompanyId { get; set; }
        public long DepartmentId { get; set; }
        public long ShiftId { get; set; }
        public long ContractorId { get; set; }
    }
}
