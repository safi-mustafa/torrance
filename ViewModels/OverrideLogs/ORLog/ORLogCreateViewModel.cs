﻿using System.ComponentModel.DataAnnotations;
using ViewModels.Authentication;
using ViewModels.Authentication.User;
using ViewModels.Common.Company;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.Shared;
using ViewModels.TimeOnTools.Shift;
using ViewModels.WeldingRodRecord;

namespace ViewModels.OverrideLogs.ORLog
{
    public class ORLogCreateViewModel : BaseCreateVM, IBaseCrudViewModel, IORLogCost
    {

        [Display(Name = "Completed")]
        public DateTime WorkCompletedDate { get; set; } = DateTime.Now;

        [Display(Name = "PO Number")]

        [Range(1, long.MaxValue, ErrorMessage = "The PO Number must be greater than zero.")]
        public long PoNumber { get; set; }
        [Required]
        public string Description { get; set; }

        [Display(Name = "Workscope")]
        public string? WorkScope { get; set; }

        public UnitBriefViewModel Unit { get; set; } = new();

        public ShiftBriefViewModel Shift { get; set; } = new ShiftBriefViewModel();

        public ReasonForRequestBriefViewModel ReasonForRequest { get; set; } = new ReasonForRequestBriefViewModel();

        public EmployeeBriefViewModel Employee { get; set; } = new();
        public CompanyBriefViewModel Company { get; set; } = new();

        public ApproverBriefViewModel Approver { get; set; } = new ApproverBriefViewModel(false);

        public DepartmentBriefViewModel Department { get; set; } = new DepartmentBriefViewModel(true);

        public List<ORLogCostViewModel> Costs { get; set; } = new List<ORLogCostViewModel>();

    }
}
