using Enums;
using Helpers.Models.Shared;
using Models.Common;
using Models.Common.Interfaces;
using Models.OverrideLogs;
using Models.WeldingRodRecord;
using System.ComponentModel.DataAnnotations.Schema;
using System.Timers;

namespace Models.TimeOnTools
{
    public class TOTLog : BaseDBModel, IApprove, IEmployeeId, IApproverId, IUnitId, ICompanyId, IDepartmentId
    {
        public DateTime? Date { get; set; }
        public string Twr { get; set; }

        public string? WorkScope { get; set; }
        public double? ManHours { get; set; }
        public DateTime? StartOfWork { get; set; }
        public TimeSpan? TimeRequested { get; set; }
        public TimeSpan? TimeSigned { get; set; }

        [ForeignKey("ReasonForRequest")]
        public long? ReasonForRequestId { get; set; }
        public ReasonForRequest? ReasonForRequest { get; set; }
        public string? JobDescription { get; set; }
        public string? Comment { get; set; }
        public long ManPowerAffected { get; set; }
        public string? EquipmentNo { get; set; }
        public double HoursDelayed { get; set; }
        public Status Status { get; set; }
        public bool IsArchived { get; set; }

        [ForeignKey("Department")]
        public long? DepartmentId { get; set; }
        public Department? Department { get; set; }

        [ForeignKey("Unit")]
        public long UnitId { get; set; }
        public Unit Unit { get; set; }

        [ForeignKey("Contractor")]
        public long? ContractorId { get; set; }
        public Contractor? Contractor { get; set; }

        [ForeignKey("StartOfWorkDelay")]
        public long? StartOfWorkDelayId { get; set; }
        public StartOfWorkDelay? StartOfWorkDelay { get; set; }


        [ForeignKey("ShiftDelay")]
        public long? ShiftDelayId { get; set; }
        public ShiftDelay? ShiftDelay { get; set; }

        [ForeignKey("ReworkDelay")]
        public long? ReworkDelayId { get; set; }
        public ReworkDelay? ReworkDelay { get; set; }

        [ForeignKey("OngoingWorkDelay")]
        public long? OngoingWorkDelayId { get; set; }
        public OngoingWorkDelay? OngoingWorkDelay { get; set; }

        public string? PermitNo { get; set; }

        public string? DelayDescription { get; set; }

        [ForeignKey("PermitType")]
        public long PermitTypeId { get; set; }
        public PermitType PermitType { get; set; }

        [ForeignKey("PermittingIssue")]
        public long? PermittingIssueId { get; set; }
        public PermittingIssue? PermittingIssue { get; set; }

        public DelayReasonCatalog? DelayReason { get; set; }

        [ForeignKey("Shift")]
        public long ShiftId { get; set; }
        public Shift Shift { get; set; }

        [ForeignKey("Approver")]
        public long? ApproverId { get; set; }
        public ToranceUser? Approver { get; set; }
        public string? Foreman { get; set; }

        [ForeignKey("Employee")]
        public long? EmployeeId { get; set; }
        public ToranceUser? Employee { get; set; }

        [ForeignKey("Company")]
        public long CompanyId { get; set; }
        public Company Company { get; set; }

        [ForeignKey("DelayType")]
        public long? DelayTypeId { get; set; }
        public DelayType? DelayType { get; set; }
    }
}
