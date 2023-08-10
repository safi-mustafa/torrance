using System;
using Models.Common;
using Models.WeldingRodRecord;
using System.ComponentModel.DataAnnotations.Schema;
using Helpers.Models.Shared;
using Models.Common.Interfaces;
using Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Models
{
    public class FCOLog : BaseDBModel, IApprove, IEmployeeId, IApproverId, IUnitId, IDepartmentId, ICompanyId
    {
        public FCOLog()
        {
        }

        public Status Status { get; set; }
        public string? DescriptionOfFinding { get; set; }
        public string? AdditionalInformation { get; set; }
        public string? EquipmentNumber { get; set; }
        public string? Location { get; set; }
        public bool ShutdownRequired { get; set; }
        public bool ScaffoldRequired { get; set; }
        public long SrNo { get; set; }
        public bool PAndIdAttached { get; set; }
        public bool ISOAttached { get; set; }
        public bool DrawingsAttached { get; set; }
        public bool ScheduleImpact { get; set; }
        public long DaysImpacted { get; set; }
        public DateTime? Date { get; set; }
        public bool AnalysisOfAlternatives { get; set; }
        public bool EquipmentFailureReport { get; set; }
        public DuringExecutionCatalog? DuringExecution { get; set; }
        public double TotalCost { get; set; }
        public double TotalHours { get; set; }
        public double TotalHeadCount { get; set; }

        [ForeignKey("Employee")]
        public long? EmployeeId { get; set; }
        public ToranceUser? Employee { get; set; }

        [ForeignKey("Company")]
        public long CompanyId { get; set; }
        public Company Company { get; set; }

        [ForeignKey("Department")]
        public long DepartmentId { get; set; }
        public Department Department { get; set; }

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

        //[ForeignKey("Company")]
        //public long CompanyId { get; set; }
        //public Company Company { get; set; }

        [ForeignKey("Approver")]
        public long? ApproverId { get; set; }
        public ToranceUser? Approver { get; set; }

        [ForeignKey("AuthorizerForImmediateStart")]
        public long? AuthorizerForImmediateStartId { get; set; }
        public ToranceUser? AuthorizerForImmediateStart { get; set; }
        public DateTime? AuthorizerForImmediateStartDate { get; set; }

        [ForeignKey("RLTMember")]
        public long? RLTMemberId { get; set; }
        public ToranceUser? RLTMember { get; set; }
        public DateTime? RLTMemberApproveDate { get; set; }

        [ForeignKey("BTLApprover")]
        public long? BTLApproverId { get; set; }
        public ToranceUser? BTLApprover { get; set; }
        public DateTime? BTLApproveDate { get; set; }

        [ForeignKey("TELApprover")]
        public long? TELApproverId { get; set; }
        public ToranceUser? TELApprover { get; set; }
        public DateTime? TELApprovalDate { get; set; }

        [ForeignKey("MaintManager")]
        public long? MaintManagerId { get; set; }
        public ToranceUser? MaintManager { get; set; }
        public DateTime? MaintManagerApprovalDate { get; set; }

        public List<FCOSection> FCOSections { get; set; }

    }
}

