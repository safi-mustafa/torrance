﻿using Enums;
using ViewModels.Authentication.User;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;

namespace ViewModels.Authentication.Approver
{
    public class ApproverSearchViewModel : UserSearchViewModel
    {
        public UnitBriefViewModel Unit { get; set; } = new();
        public DepartmentBriefViewModel Department { get; set; } = new();
        public ActiveStatus? ActiveStatus { get; set; }

        public bool IsSearchForm { get; set; }
        public FilterLogType LogType { get; set; }
    }
}
