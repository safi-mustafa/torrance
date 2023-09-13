using Enums;
using Pagination;
using System.ComponentModel.DataAnnotations;
using ViewModels.Authentication.User;
using ViewModels.Common.Company;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.WeldingRodRecord.Location;

namespace ViewModels.WeldingRodRecord.WRRLog
{
    public class WRRLogSearchViewModel : BaseSearchModel
    {
        public WRRLogSearchViewModel()
        {
            OrderByColumn = "CreatedOn";
            OrderDir = PaginationOrderCatalog.Desc;
        }
        public string Email { get; set; }
        public bool IsExcelDownload { get; set; }
        public CompanyBriefViewModel Company { get; set; } = new CompanyBriefViewModel();
        public DepartmentBriefViewModel Department { get; set; } = new DepartmentBriefViewModel();
        [Display(Name = "Requestor")]

        public EmployeeBriefViewModel Requestor { get; set; } = new EmployeeBriefViewModel();

        public UnitBriefViewModel Unit { get; set; } = new UnitBriefViewModel();

        public LocationBriefViewModel Location { get; set; } = new LocationBriefViewModel();

        public StatusSearchEnum? Status { get; set; } = null;

        [Display(Name = "Status Is Not")]
        public Status? StatusNot { get; set; } = null;

        [Display(Name = "Show Archived")]
        public bool IsArchived { get; set; }

        public ApproverBriefViewModel Approver { get; set; } = new ApproverBriefViewModel(false);
        public List<string> SelectedIds { get; set; }
    }

    public class WRRLogAPISearchViewModel : BaseSearchModel
    {
        public string Email { get; set; }

        public long DepartmentId { get; set; }

        public long RequestorId { get; set; }

        public long UnitId { get; set; }

        public long LocationId { get; set; }

        public Status? Status { get; set; } = null;
        public long CompanyId { get; set; }
        public long ApproverId { get; set; }
        public List<string> SelectedIds { get; set; }
    }
}
