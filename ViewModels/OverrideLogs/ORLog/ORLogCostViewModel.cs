using Enums;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.OverrideLogs.ORLog
{
    public class ORLogCostViewModel
    {
        public long Id { get; set; }

        [Display(Name = "Hours")]
        public double? OverrideHours { get; set; }
        [Display(Name = "Override Type")]
        public OverrideTypeCatalog? OverrideType { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The field Head Count must be greater than zero.")]
        [Required]
        public int? HeadCount { get; set; }
        [RequiredNotNull]
        public CraftSkillForORLogBriefViewModel CraftSkill { get; set; } = new();


        [Display(Name = "ST Hours")]
        public double? STHours { get; set; } = 0;

        [Display(Name = "OT Hours")]
        public double? OTHours { get; set; } = 0;

        [Display(Name = "DT Hours")]
        public double? DTHours { get; set; } = 0;

        public long OverrideLogId { get; set; }

        public double Cost
        {
            get
            {
                double totalCost = 0d;
                if (OverrideType != null)
                {
                    var rate = (OverrideType == OverrideTypeCatalog.ST ? CraftSkill.STRate : (OverrideType == OverrideTypeCatalog.OT ? CraftSkill.OTRate : CraftSkill.DTRate));
                    totalCost = ((rate ?? 0) * (OverrideHours ?? 0) * (HeadCount ?? 0));
                }
                else
                {
                    totalCost = ((CraftSkill.STRate * STHours) + (CraftSkill.OTRate * OTHours) + (CraftSkill.DTRate * DTHours)) ?? 0;
                    totalCost = (totalCost * HeadCount) ?? 0;
                }
                // Round the totalCost to 2 decimal places
                totalCost = Math.Round(totalCost, 2);

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
