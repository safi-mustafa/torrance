﻿using Enums;
using Pagination;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.WeldingRodRecord.Employee;
using ViewModels.WeldingRodRecord.Location;
using ViewModels.WeldingRodRecord.RodType;
using ViewModels.WeldingRodRecord.WeldMethod;

namespace ViewModels.WeldingRodRecord.WRRLog
{
    public class WRRLogSearchViewModel : BaseSearchModel
    {
        public string Email { get; set; }

        public DepartmentBriefViewModel Department { get; set; } = new DepartmentBriefViewModel();

        public EmployeeBriefViewModel Employee { get; set; } = new EmployeeBriefViewModel();

        public UnitBriefViewModel Unit { get; set; } = new UnitBriefViewModel();

        public LocationBriefViewModel Location { get; set; } = new LocationBriefViewModel();

        public StatusSearchEnum? Status { get; set; } = null;

        [Display(Name = "Status Is Not")]
        public Status? StatusNot { get; set; } = null;
    }

    public class WRRLogAPISearchViewModel : BaseSearchModel
    {
        public string Email { get; set; }

        public long DepartmentId { get; set; }

        public long EmployeeId { get; set; }

        public long UnitId { get; set; }

        public long LocationId { get; set; }

        public Status Status { get; set; }

    }
}
