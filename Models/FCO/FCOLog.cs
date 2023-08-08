using System;
using Models.Common;
using Models.WeldingRodRecord;
using System.ComponentModel.DataAnnotations.Schema;
using Helpers.Models.Shared;
using Models.Common.Interfaces;
using Enums;

namespace Models
{
    public class FCOLog : BaseDBModel, IApprove, IEmployeeId, IUnitId, ICompanyId, IDepartmentId
    {
        public FCOLog()
        {
        }

        public Status Status { get; set; }
        public string DescriptionOfFinding { get; set; }
        public string AdditionalInformation { get; set; }
        public long SrNo { get; set; }
        public bool PAndIdAttached { get; set; }
        public bool ISOAttached { get; set; }
        public bool DrawingsAttached { get; set; }
        public bool ScheduleImpact { get; set; }
        public long DaysImpacted { get; set; }

        [ForeignKey("Employee")]
        public long? EmployeeId { get; set; }
        public ToranceUser? Employee { get; set; }

        [ForeignKey("Department")]
        public long DepartmentId { get; set; }
        public Department Department { get; set; }

        [ForeignKey("Unit")]
        public long UnitId { get; set; }
        public Unit Unit { get; set; }

        [ForeignKey("Location")]
        public long LocationId { get; set; }
        public Location Location { get; set; }

        [ForeignKey("Contractor")]
        public long? ContractorId { get; set; }
        public Contractor? Contractor { get; set; }

        [ForeignKey("Company")]
        public long CompanyId { get; set; }
        public Company Company { get; set; }

        [ForeignKey("AuthorizerForImmediateStart")]
        public long? AuthorizerForImmediateStartId { get; set; }
        public ToranceUser? AuthorizerForImmediateStart { get; set; }
        public DateTime AuthorizerForImmediateStartDate { get; set; }

        [ForeignKey("DesignatedCoordinator")]
        public long? DesignatedCoordinatorId { get; set; }
        public ToranceUser? DesignatedCoordinator { get; set; }
        public DateTime DesignatedCoordinationDate { get; set; }

        [ForeignKey("EndorserBTL")]
        public long? EndorserBTLId { get; set; }
        public ToranceUser? EndorserBTL { get; set; }
        public DateTime EndorsmentBTLDate { get; set; }

        [ForeignKey("EndorserUnitSuperindendant")]
        public long? EndorserUnitSuperindendantId { get; set; }
        public ToranceUser? EndorserUnitSuperindendant { get; set; }
        public DateTime EndorsmentUnitSuperindendantDate { get; set; }


        public List<FCOSection> FCOSections { get; set; }

    }
}

