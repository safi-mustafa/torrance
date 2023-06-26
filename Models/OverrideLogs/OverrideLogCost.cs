using Enums;
using Helpers.Models.Shared;
using Models.Common;
using Models.Common.Interfaces;
using Models.TimeOnTools;
using Models.WeldingRodRecord;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.OverrideLogs
{
    public class OverrideLogCost
    {
        [Key]
        public long Id { get; set; }
        public long OverrideLogId { get; set; }
        public OverrideLog OverrideLog { get; set; }

        public int? HeadCount { get; set; }
        public OverrideTypeCatalog? OverrideType { get; set; }

        [ForeignKey("CraftSkill")]
        public long? CraftSkillId { get; set; }
        public CraftSkill? CraftSkill { get; set; }
        public double? OverrideHours { get; set; }


    }
}
