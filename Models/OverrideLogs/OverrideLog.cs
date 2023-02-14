using Enums;
using Helpers.Models.Shared;
using Models.Common;
using Models.Common.Interfaces;
using Models.TimeOnTools;
using Models.WeldingRodRecord;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.OverrideLogs
{
    public class OverrideLog : BaseDBModel, IApprove, IEmployeeId, IApproverId
    {
        public DateTime WorkCompletedDate { get; set; }
        public string? WorkScope { get; set; }
        public string? Description { get; set; }
        public int OverrideHours { get; set; }
        public long PoNumber { get; set; }
        public Status Status { get; set; }

        [ForeignKey("Shift")]
        public long ShiftId { get; set; }
        public Shift Shift { get; set; }

        [ForeignKey("Unit")]
        public long UnitId { get; set; }
        public Unit Unit { get; set; }

        [ForeignKey("Department")]
        public long? DepartmentId { get; set; }
        public Department? Department { get; set; }

        [ForeignKey("ReasonForRequest")]
        public long ReasonForRequestId { get; set; }
        public ReasonForRequest ReasonForRequest { get; set; }

        [ForeignKey("Company")]
        public long CompanyId { get; set; }
        public Company Company { get; set; }

        [ForeignKey("Employee")]
        public long? EmployeeId { get; set; }
        public ToranceUser? Employee { get; set; }

        [ForeignKey("Contractor")]
        public long? ContractorId { get; set; }
        public Contractor? Contractor { get; set; }

        [ForeignKey("Approver")]
        public long? ApproverId { get; set; }
        public ToranceUser? Approver { get; set; }

        public double TotalCost { get; set; }

        public double TotalHours { get; set; }
    }
}
