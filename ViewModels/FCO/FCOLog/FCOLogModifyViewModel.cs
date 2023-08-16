using System.ComponentModel.DataAnnotations;
using Models.Common.Interfaces;
using ViewModels.Shared;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.Common.Contractor;
using Enums;
using ViewModels.Authentication.User;
using ViewModels.WeldingRodRecord;
using ViewModels.Common.Company;
using Helpers.Double;
using Models.Common;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ViewModels
{
    public class FCOLogModifyViewModel : BaseUpdateVM, IBaseCrudViewModel, ISrNo, IFCOLogAttachment<AttachmentModifyViewModel>, IIdentitifier, IApprove
    {
        public Status Status { get; set; }
        [Display(Name = "Description of Finding (Attach marked-up P&ID or iso / Sketch)")]
        public string? DescriptionOfFinding { get; set; }
        [Display(Name = "Additional Information")]
        public string? AdditionalInformation { get; set; }
        [Display(Name = "Loop Identification # & Equipment Number")]
        [Required]
        public string? EquipmentNumber { get; set; }
        [Display(Name = "Service/Location")]
        [Required]
        public string? Location { get; set; }
        [Display(Name = "Shutdown Required")]
        public bool ShutdownRequired { get; set; }

        [Display(Name = "PreTA")]
        public bool PreTA { get; set; }
        [Display(Name = "Scaffold Required")]
        public bool ScaffoldRequired { get; set; }
        [Display(Name = "FCO No.")]
        public long SrNo { get; set; }
        [Display(Name = "Analysis of alternatives / Mitigation Options (Y/N)")]
        public bool AnalysisOfAlternatives { get; set; }
        [Display(Name = "Equipment Failure Report")]
        public bool EquipmentFailureReport { get; set; }
        [Display(Name = "Drawings Attached")]
        public bool DrawingsAttached { get; set; }
        [Display(Name = "Schedule Impact")]
        public bool ScheduleImpact { get; set; }
        [Display(Name = "Days Impact")]
        public long DaysImpacted { get; set; }
        [Display(Name = "During Execution")]
        public DuringExecutionCatalog? DuringExecution { get; set; }
        [Required]
        public DateTime? Date { get; set; }
        [Display(Name = "Contingency (%)")]
        public double Contingency { get; set; } = 10;

        [Display(Name = "Material Name")]
        public string? MaterialName { get; set; }
        [Display(Name = "Material Rate")]
        public double MaterialRate { get; set; }
        [Display(Name = "Equipment Name")]
        public string? EquipmentName { get; set; }
        [Display(Name = "Equipment Rate")]
        public double EquipmentRate { get; set; }
        [Display(Name = "Shop Name")]
        public string? ShopName { get; set; }
        [Display(Name = "Shop Rate")]
        public double ShopRate { get; set; }

        public ContractorBriefViewModel Contractor { get; set; } = new();
        public CompanyBriefViewModel Company { get; set; } = new(true, "The Company field is required.");
        public EmployeeBriefViewModel? Employee { get; set; } = new();
        public DepartmentBriefViewModel Department { get; set; } = new();
        public UnitBriefViewModel Unit { get; set; } = new(true);
        public FCOTypeBriefViewModel? FCOType { get; set; } = new(true);
        public FCOReasonBriefViewModel? FCOReason { get; set; } = new(true);
        public AreaExecutionLeadBriefViewModel? AreaExecutionLead { get; set; } = new(false);
        public DateTime? AreaExecutionLeadApprovalDate { get; set; }
        public RejecterBriefViewModel? BusinessTeamLeader { get; set; } = new(false);
        public DateTime? BusinessTeamLeaderApprovalDate { get; set; }
        public RejecterBriefViewModel? Rejecter { get; set; } = new(false);
        public DateTime? RejecterDate { get; set; }

        public double TotalCost { get => FCOSections.Sum(x => x.Estimate); }
        public double TotalHours { get => FCOSections.Sum(x => x.DU); }
        public double TotalHeadCount { get => FCOSections.Sum(x => x.MN); }

        public List<FCOSectionModifyViewModel>? FCOLabourSections { get; set; } = new();
        public List<FCOSectionModifyViewModel>? FCOMaterialSections { get; set; } = new();
        public List<FCOSectionModifyViewModel>? FCOEquipmentSections { get; set; } = new();
        public List<FCOSectionModifyViewModel>? FCOShopSections { get; set; } = new();
        private List<FCOSectionModifyViewModel>? _fCOSections;
        [BindNever]
        public List<FCOSectionModifyViewModel>? FCOSections
        {
            get
            {
                if (_fCOSections == null || _fCOSections.Count < 1)
                    return FCOLabourSections?.Concat(FCOMaterialSections?.Concat(FCOEquipmentSections?.Concat(FCOShopSections))).ToList() ?? new List<FCOSectionModifyViewModel>();
                return _fCOSections;
            }
            set => _fCOSections = value;
        }
        public AttachmentModifyViewModel? Photo { get; set; } = new(AttachmentEntityType.FCOLogPhoto);

        public AttachmentModifyViewModel? File { get; set; } = new(AttachmentEntityType.FCOLogFile);
        public double Total { get => Math.Round(FCOSections.Where(x => x.SectionType != FCOSectionCatalog.Shop).Sum(x => x.Estimate), 2).FixNan(); }
        public double Contingencies
        {
            get
            {
                return Math.Round((Total / Contingency), 2).FixNan();
            }
        }
        [Display(Name = "Total")]
        public double SubTotal { get => Math.Round(Total + Contingencies, 2).FixNan(); }
        public double TotalLabor { get { var laborEstimate = FCOSections.Where(x => x.SectionType == FCOSectionCatalog.Labour).Sum(x => x.Estimate); return laborEstimate + (laborEstimate / Contingency).FixNan(); } }
        public double TotalMaterial { get { var materialEstimate = FCOSections.Where(x => x.SectionType == FCOSectionCatalog.Material).Sum(x => x.Estimate); return materialEstimate + (materialEstimate / Contingency).FixNan(); } }
        public double TotalEquipment { get { var equipmentEstimate = FCOSections.Where(x => x.SectionType == FCOSectionCatalog.Equipment).Sum(x => x.Estimate); return equipmentEstimate + (equipmentEstimate / Contingency).FixNan(); } }
        public double TotalShop { get { var shopEstimate = FCOSections.Where(x => x.SectionType == FCOSectionCatalog.Shop).Sum(x => x.Estimate); return shopEstimate + (shopEstimate / Contingency).FixNan(); } }
        public double SectionTotal { get => TotalLabor + TotalMaterial + TotalEquipment + TotalShop; }

    }
}
