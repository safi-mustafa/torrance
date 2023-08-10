using System;
using Enums;
using Models;
using Models.Common;
using System.ComponentModel.DataAnnotations.Schema;
using ViewModels.Shared;
using ViewModels.Common.Contractor;

namespace ViewModels
{
    public class FCOSectionCreateViewModel : BaseCreateVM, IBaseCrudViewModel

    {
        public FCOSectionCreateViewModel()
        {
        }
        public FCOSectionCatalog SectionType { get; set; }
        public double DU { get; set; }
        public double MN { get; set; }
        public double Rate { get; set; }
        public double Estimate { get => DU * MN * Rate; }
        public ContractorBriefViewModel? Contractor { get; set; } = new();

        public FCOLogBriefViewModel FCOLog { get; set; } = new();
    }
}

