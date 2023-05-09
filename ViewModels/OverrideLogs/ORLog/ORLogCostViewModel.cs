using Enums;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.OverrideLogs.ORLog
{
    public class ORLogCostViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Hours")]
        public int OverrideHours { get; set; }

        public int HeadCount { get; set; }
        public CraftSkillBriefViewModel CraftSkill { get; set; } = new CraftSkillBriefViewModel();

        public OverrideTypeCatalog OverrideType { get; set; }

        public long OverrideLogId { get; set; }

        public double Cost
        {
            get
            {
                var rate = (OverrideType == OverrideTypeCatalog.ST ? CraftSkill.STRate : (OverrideType == OverrideTypeCatalog.OT ? CraftSkill.OTRate : CraftSkill.DTRate));
                var totalCost = ((rate ?? 0) * OverrideHours * HeadCount);
                return totalCost;
            }
        }
        public double CraftRate
        {
            get
            {
                return (OverrideType == OverrideTypeCatalog.ST ? CraftSkill.STRate : (OverrideType == OverrideTypeCatalog.OT ? CraftSkill.OTRate : CraftSkill.DTRate)) ?? 0;
            }
        }
        public string FormattedCraftRate
        {
            get
            {
                return string.Format("{0:C}", CraftRate);
            }
        }
        public string FormattedCost
        {
            get
            {
                return string.Format("{0:C}", Cost);
            }
        }
    }
}
