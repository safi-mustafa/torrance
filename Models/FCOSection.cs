using System;
using Models.Common;
using System.ComponentModel.DataAnnotations.Schema;
using Helpers.Models.Shared;
using Enums;
using Models.OverrideLogs;

namespace Models
{
    public class FCOSection : BaseDBModel
    {
        public FCOSection()
        {
        }

        public string? Name { get; set; }
        public FCOSectionCatalog SectionType { get; set; }
        public double DU { get; set; }
        public double MN { get; set; }
        public double Rate { get; set; }
        public double Estimate { get; set; }
        public OverrideTypeCatalog? OverrideType { get; set; }

        [ForeignKey("Craft")]
        public long? CraftId { get; set; }
        public CraftSkill? Craft { get; set; }

        [ForeignKey("Contractor")]
        public long? ContractorId { get; set; }
        public Contractor? Contractor { get; set; }

        [ForeignKey("FCOLog")]
        public long FCOLogId { get; set; }
        public FCOLog FCOLog { get; set; }

    }
}

