using System;
using System.ComponentModel.DataAnnotations;
using Enums;
using Pagination;
using ViewModels.WeldingRodRecord;

namespace ViewModels.Common
{
    public class ApprovalSearchViewModel : BaseSearchModel
    {
        public ApprovalSearchViewModel()
        {
        }
        public LogType? Type { get; set; }
        public Status Status { get; set; } = Status.Pending;

        [Display(Name = "Requestor")]
        public EmployeeBriefViewModel Employee { get; set; } = new();
    }
}

