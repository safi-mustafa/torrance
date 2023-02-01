using Enums;
using Helpers.Models.Shared;
using Models.Common;
using Models.Common.Interfaces;
using Models.TimeOnTools;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.OverrideLogs
{
    public class OverrideLog : BaseDBModel, IApprove
    {
        public string Requester { get; set; }
        public string RequesterEmail { get; set; }
        public DateTime DateSubmitted { get; set; }
        public TimeSpan TimeSubmitted { get; set; }
        public DateTime DateOfWorkCompleted { get; set; }
        public string WorkScope { get; set; }
        public int OverrideHours { get; set; }
        public long PONumber { get; set; }
        public Status Status { get; set; }

        [ForeignKey("Shift")]
        public long ShiftId { get; set; }
        public Shift Shift { get; set; }

        [ForeignKey("ReasonForRequest")]
        public long ReasonForRequestId { get; set; }
        public ReasonForRequest ReasonForRequest { get; set; }

        [ForeignKey("CraftRate")]
        public long CraftRateId { get; set; }
        public CraftRate CraftRate { get; set; }

        [ForeignKey("CraftSkill")]
        public long CraftSkillId { get; set; }
        public CraftSkill CraftSkill { get; set; }

        [ForeignKey("OverrideType")]
        public long OverrideTypeId { get; set; }
        public OverrideType OverrideType { get; set; }

        [ForeignKey("Contractor")]
        public long ContractorId { get; set; }
        public Contractor Contractor { get; set; }

        public List<OverrideLogEmployee> Employees { get; set; }
    }
}
