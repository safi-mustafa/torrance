using Models.Common;
using Models.WeldingRodRecord;
using System.ComponentModel.DataAnnotations.Schema;
using Helpers.Models.Shared;
using Models.Common.Interfaces;
using Enums;
using Models.FCO;

namespace Models
{
    public class FCOLog : BaseDBModel, IApprove, IEmployeeId, IUnitId, ICompanyId, IApprovalDate
    {

        public Status Status { get; set; }
        public string? DescriptionOfFinding { get; set; }
        public string? AdditionalInformation { get; set; }
        public string? EquipmentNumber { get; set; }
        public string? Location { get; set; }
        public bool ShutdownRequired { get; set; }

        public bool PreTA { get; set; }
        public bool ScaffoldRequired { get; set; }
        public long SrNo { get; set; }
        public bool PAndIdAttached { get; set; }
        public bool ISOAttached { get; set; }
        public bool DrawingsAttached { get; set; }
        public bool ScheduleImpact { get; set; }
        public long DaysImpacted { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public bool AnalysisOfAlternatives { get; set; }
        public bool EquipmentFailureReport { get; set; }
        public DuringExecutionCatalog? DuringExecution { get; set; }
        public double TotalCost { get; set; }
        public double TotalHours { get; set; }
        public double TotalHeadCount { get; set; }
        public double Contingency { get; set; }


        [ForeignKey("DesignatedCoordinator")]
        public long? DesignatedCoordinatorId { get; set; }
        public ToranceUser? DesignatedCoordinator { get; set; }

        [ForeignKey("Employee")]
        public long? EmployeeId { get; set; }
        public ToranceUser? Employee { get; set; }

        [ForeignKey("Company")]
        public long CompanyId { get; set; }
        public Company Company { get; set; }

        [ForeignKey("Department")]
        public long? DepartmentId { get; set; }
        public Department? Department { get; set; }

        [ForeignKey("Unit")]
        public long UnitId { get; set; }
        public Unit Unit { get; set; }

        [ForeignKey("Contractor")]
        public long? ContractorId { get; set; }
        public Contractor? Contractor { get; set; }

        [ForeignKey("FCOType")]
        public long? FCOTypeId { get; set; }
        public FCOType? FCOType { get; set; }

        [ForeignKey("FCOReason")]
        public long? FCOReasonId { get; set; }
        public FCOReason? FCOReason { get; set; }

        [ForeignKey("AreaExecutionLead")]
        public long? AreaExecutionLeadId { get; set; }
        public ToranceUser? AreaExecutionLead { get; set; }
        public DateTime? AreaExecutionLeadApprovalDate { get; set; }

        [ForeignKey("BusinessTeamLeader")]
        public long? BusinessTeamLeaderId { get; set; }
        public ToranceUser? BusinessTeamLeader { get; set; }
        public DateTime? BusinessTeamLeaderApprovalDate { get; set; }

        [ForeignKey("Rejecter")]
        public long? RejecterId { get; set; }
        public ToranceUser? Rejecter { get; set; }
        public DateTime? RejecterDate { get; set; }

        public List<FCOSection> FCOSections { get; set; }
        public List<FCOComment> FCOComments { get; set; }

    }
}

