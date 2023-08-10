using Select2.Model;
using System.ComponentModel;

namespace ViewModels.OverrideLogs
{
    public class CraftSkillBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public CraftSkillBriefViewModel() : base(false, "The Craft Skill field is required.")
        {

        }
        [DisplayName("Craft Skill")]
        public override string? Name { get; set; }

        public double? STRate { get; set; }

        public double? OTRate { get; set; }

        public double? DTRate { get; set; }
    }
    public class CraftSkillForORLogBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public CraftSkillForORLogBriefViewModel() : base(true, "The Craft Skill field is required.")
        {

        }
        [DisplayName("Craft Skill")]
        public override string? Name { get; set; }

        public double? STRate { get; set; }

        public double? OTRate { get; set; }

        public double? DTRate { get; set; }
    }
    public class CraftSkillForFCOLogBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public CraftSkillForFCOLogBriefViewModel() : base(false, "The Craft Skill field is required.")
        {

        }
        [DisplayName("Craft Skill")]
        public override string? Name { get; set; }

        public double? STRate { get; set; }

        public double? OTRate { get; set; }

        public double? DTRate { get; set; }
    }

}
