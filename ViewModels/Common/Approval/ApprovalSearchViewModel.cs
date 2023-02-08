using System;
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
        public EmployeeBriefViewModel Employee { get; set; } = new();
    }
}

