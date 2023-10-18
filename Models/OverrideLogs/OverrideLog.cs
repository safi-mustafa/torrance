using Enums;
using Helpers.Models.Shared;
using Models.Common;
using Models.Common.Interfaces;
using Models.TimeOnTools;
using Models.WeldingRodRecord;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.OverrideLogs
{
    public class OverrideLog : BaseDBModel, IApprove, IEmployeeId, IApproverId, IUnitId, ICompanyId, IDepartmentId
    {
        public DateTime WorkCompletedDate { get; set; }
        public string? WorkScope { get; set; }
        public string? Description { get; set; }
        public int OverrideHours { get; set; }
        public long PoNumber { get; set; }
        public Status Status { get; set; }
        public bool IsArchived { get; set; }

        [ForeignKey("Shift")]
        public long ShiftId { get; set; }
        public Shift Shift { get; set; }

        [ForeignKey("Unit")]
        public long UnitId { get; set; }
        public Unit Unit { get; set; }

        [ForeignKey("Department")]
        public long DepartmentId { get; set; }
        public Department Department { get; set; }

        [ForeignKey("ReasonForRequest")]
        public long? ReasonForRequestId { get; set; }
        public ReasonForRequest? ReasonForRequest { get; set; }

        [ForeignKey("DelayType")]
        public long? DelayTypeId { get; set; }
        public DelayType? DelayType { get; set; }

        public DelayReasonCatalog? DelayReason { get; set; }

        [ForeignKey("StartOfWorkDelay")]
        public long? StartOfWorkDelayId { get; set; }
        public StartOfWorkDelay? StartOfWorkDelay { get; set; }


        [ForeignKey("ShiftDelay")]
        public long? ShiftDelayId { get; set; }
        public ShiftDelay? ShiftDelay { get; set; }

        [ForeignKey("ReworkDelay")]
        public long? ReworkDelayId { get; set; }
        public ReworkDelay? ReworkDelay { get; set; }

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

        public double TotalHeadCount { get; set; }

        public string? Reason { get; set; }

        public string? EmployeeNames { get; set; }

        public string? ClippedEmployeesUrl { get; set; }

        public List<OverrideLogCost> Costs { get; set; }
    }
}
