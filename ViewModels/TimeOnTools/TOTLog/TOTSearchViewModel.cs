﻿using Enums;
using Pagination;
using System.ComponentModel.DataAnnotations;
using ViewModels.Authentication;
using ViewModels.Authentication.User;
using ViewModels.Common.Company;
using ViewModels.Common.Contractor;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.TimeOnTools.PermitType;
using ViewModels.TimeOnTools.Shift;
using ViewModels.WeldingRodRecord;

namespace ViewModels.TimeOnTools.TOTLog
{
    public class TOTLogSearchViewModel : BaseSearchModel
    {
        public TOTLogSearchViewModel()
        {
            OrderByColumn = "CreatedOn";
            OrderDir = PaginationOrderCatalog.Desc;
        }
        [Display(Name = "Equipment No")]
        public string? EquipmentNo { get; set; }
        public StatusSearchEnum? Status { get; set; } = null;
        [Display(Name = "Status Is Not")]
        public List<Status> StatusNot { get; set; } = new();
        public UnitBriefViewModel Unit { get; set; } = new UnitBriefViewModel(false);
        public ShiftBriefViewModel Shift { get; set; } = new ShiftBriefViewModel(false);
        public DelayTypeBriefViewModel DelayType { get; set; } = new DelayTypeBriefViewModel(false);
        public PermitTypeBriefViewModel PermitType { get; set; } = new PermitTypeBriefViewModel(false);
        public EmployeeBriefViewModel Requestor { get; set; } = new EmployeeBriefViewModel(false, "");
        public ApproverBriefViewModel Approver { get; set; } = new ApproverBriefViewModel(false, "");
        public DepartmentBriefViewModel Department { get; set; } = new DepartmentBriefViewModel(false);
        public CompanyBriefViewModel Company { get; set; } = new CompanyBriefViewModel();
        public bool IsExcelDownload { get; set; }
        [Display(Name = "Show Archived")]
        public bool IsArchived { get; set; }
        public List<string> SelectedIds { get; set; }
    }

    public class TOTLogAPISearchViewModel : BaseSearchModel
    {
        public long? EquipmentNo { get; set; }
        public long? UnitId { get; set; }
        public long? ShiftId { get; set; }
        public long? DelayTypeId { get; set; }
        public long? PermitTypeId { get; set; }
        public long? RequestorId { get; set; }
        public long? ApproverId { get; set; }
        public long CompanyId { get; set; }
        public long DepartmentId { get; set; }
        public long ContractorId { get; set; }

        public List<string> SelectedIds { get; set; }

    }
}
