using Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
    }
}
