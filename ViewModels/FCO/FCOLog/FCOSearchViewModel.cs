using Enums;
using Pagination;
using System.ComponentModel.DataAnnotations;
using ViewModels.Authentication.User;
using ViewModels.Common.Company;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.WeldingRodRecord;
using ViewModels.WeldingRodRecord.Location;

namespace ViewModels
{
    public class FCOLogSearchViewModel : BaseSearchModel
    {
        public FCOLogSearchViewModel()
        {
            OrderByColumn = "CreatedOn";
            OrderDir = PaginationOrderCatalog.Desc;
        }
        public string Email { get; set; }
        public bool IsExcelDownload { get; set; }
        public bool IsRawReport { get; set; } = false;
        public CompanyBriefViewModel Company { get; set; } = new CompanyBriefViewModel();
        public DepartmentBriefViewModel Department { get; set; } = new DepartmentBriefViewModel();
        [Display(Name = "Requestor")]

        public EmployeeBriefViewModel Requestor { get; set; } = new EmployeeBriefViewModel();

        public UnitBriefViewModel Unit { get; set; } = new UnitBriefViewModel();

        public string? Location { get; set; }
        public StatusSearchEnum? Status { get; set; } = null;

        [Display(Name = "Status Is Not")]
        public Status? StatusNot { get; set; } = null;
        public ApproverBriefViewModel Approver { get; set; } = new ApproverBriefViewModel(false);
        public List<string> SelectedIds { get; set; }
    }

    public class FCOLogAPISearchViewModel : BaseSearchModel
    {
        public string? Email { get; set; }

        public long DepartmentId { get; set; }

        public long RequestorId { get; set; }

        public long UnitId { get; set; }

        public long LocationId { get; set; }

        public Status? Status { get; set; } = null;
        public long CompanyId { get; set; }
        public long ApproverId { get; set; }
        public List<string>? SelectedIds { get; set; }
    }
}
