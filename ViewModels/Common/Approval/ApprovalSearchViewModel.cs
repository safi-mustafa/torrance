﻿using System;
using Enums;
using Pagination;
using ViewModels.WeldingRodRecord.Employee;

namespace ViewModels.Common
{
    public class ApprovalSearchViewModel : BaseSearchModel
    {
        public ApprovalSearchViewModel()
        {
        }
        public LogType? Type { get; set; }
        public Status? Status { get; set; }
        public EmployeeBriefViewModel Employee { get; set; } = new();
    }
}
