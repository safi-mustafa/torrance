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
        public string? Name { get; set; }
        public FCOSectionCatalog SectionType { get; set; }
        private double? _rate;
        public double? Rate
        {
            get
            {
                var result = SectionType == FCOSectionCatalog.Labour && (_rate == null || _rate < 1) ? (OverrideType == OverrideTypeCatalog.ST ? Craft.STRate : (OverrideType == OverrideTypeCatalog.OT ? Craft.OTRate : Craft.DTRate)) : _rate;
                return result;
            }
            set => _rate = value;
        }
        public double Estimate { get => DU * MN * Rate ?? 0; }

        //[RequiredNotNull]
        public CraftSkillForFCOLogBriefViewModel? Craft { get; set; } = new();
        //[Required]
        public OverrideTypeCatalog? OverrideType { get; set; }

        public double CraftRate
        {
            get
            {
                return (OverrideType == OverrideTypeCatalog.ST ? Craft.STRate : (OverrideType == OverrideTypeCatalog.OT ? Craft.OTRate : Craft.DTRate)) ?? 0;
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
        public FCOLogBriefViewModel? FCOLog { get; set; } = new();
    }
}

