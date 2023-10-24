using Enums;
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
        public double? STHours { get; set; }
        public double? OTHours { get; set; }
        public double? DTHours { get; set; }
    }
}
