using Enums;
using Pagination;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using ViewModels.Authentication;
using ViewModels.Common.Contractor;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;

namespace ViewModels.TomeOnTools.TOTLog
{
    public class TOTLogSearchViewModel : BaseSearchModel
    {
        [Display(Name = "Equipment No")]
        public long? EquipmentNo { get; set; }
        public Status? Status { get; set; } = null;
        public DepartmentBriefViewModel Department { get; set; } = new DepartmentBriefViewModel();

        public UnitBriefViewModel Unit { get; set; } = new UnitBriefViewModel();

        public ContractorBriefViewModel Contractor { get; set; } = new ContractorBriefViewModel();

        //public UserBriefViewModel Approver { get; set; } = new UserBriefViewModel();

        //public UserBriefViewModel Foreman { get; set; } = new UserBriefViewModel();
    }

    public class TOTLogAPISearchViewModel : BaseSearchModel
    {
        public long EquipmentNo { get; set; }
        public long DepartmentId { get; set; }

        public long UnitId { get; set; }

        public long ContractorId { get; set; }

        //public long ApproverId { get; set; }

        //public long ForemanId { get; set; }
    }
}
