using System;
using System.ComponentModel.DataAnnotations;
using Enums;
using Models.Common.Interfaces;
using Models.OverrideLogs;
using ViewModels.Common.Contractor;
using ViewModels.OverrideLogs;
using ViewModels.Shared;

namespace ViewModels
{
    public class FCOSectionModifyViewModel : BaseUpdateVM, IBaseCrudViewModel, IIdentitifier
    {
        public FCOSectionModifyViewModel()
        {
        }
        [Display(Name = "DU")]
        [Range(1, double.MaxValue, ErrorMessage = "The field DU must be greater than zero.")]
        [Required]
        public double DU { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The field MN must be greater than zero.")]
        [Required]
        public int MN { get; set; }
        public string Name { get; set; }
        public FCOSectionCatalog SectionType { get; set; }
        private double? _rate;
        public double? Rate { get => SectionType == FCOSectionCatalog.Labour ? (OverrideType == OverrideTypeCatalog.ST ? CraftSkill.STRate : (OverrideType == OverrideTypeCatalog.OT ? CraftSkill.OTRate : CraftSkill.DTRate)) : _rate; set => _rate = value; }
        public double Estimate { get => DU * MN * Rate ?? 0; }
        public long OverrideLogId { get; set; }

        [RequiredNotNull]
        public CraftSkillForORLogBriefViewModel CraftSkill { get; set; } = new();
        [Required]
        public OverrideTypeCatalog? OverrideType { get; set; }

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
        public string FormattedEstimate
        {
            get
            {
                return string.Format("{0:C}", Estimate);
            }
        }

        public ContractorBriefViewModel? Contractor { get; set; } = new();

        public FCOLogBriefViewModel FCOLog { get; set; } = new();
    }
}

