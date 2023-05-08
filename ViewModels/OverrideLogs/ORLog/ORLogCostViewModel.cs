using Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public double? Rate { get => CraftSkill != null ? (OverrideType == OverrideTypeCatalog.ST ? CraftSkill.STRate : (OverrideType == OverrideTypeCatalog.OT ? CraftSkill.OTRate : CraftSkill.DTRate)) : 0D; }

        public double TotalCost { get => ((Rate ?? 0) * OverrideHours * HeadCount); }

        public string Cost { get => string.Format("{0:C}", TotalCost); }
    }
}
