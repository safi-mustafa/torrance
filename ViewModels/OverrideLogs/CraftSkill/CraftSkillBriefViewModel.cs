﻿using Select2.Model;
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

        public double? Rate { get; set; }
    }

}
