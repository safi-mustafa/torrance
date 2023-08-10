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
using Helpers.File;
using Models.Common;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ViewModels
{
    public class FCOLogModifyViewModel : BaseUpdateVM, IBaseCrudViewModel, ISrNo, IAttachment<AttachmentModifyViewModel>, IIdentitifier, IApprove
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

        public ContractorBriefViewModel Contractor { get; set; } = new();
        public CompanyBriefViewModel Company { get; set; } = new(true, "The Company field is required.");
        public EmployeeBriefViewModel? Employee { get; set; } = new();
        public DepartmentBriefViewModel Department { get; set; } = new(true);
        public UnitBriefViewModel Unit { get; set; } = new(true);
        public FCOTypeBriefViewModel? FCOType { get; set; } = new(true);
        public FCOReasonBriefViewModel? FCOReason { get; set; } = new(true);
        public AuthorizeForImmediateStartBriefViewModel AuthorizerForImmediateStart { get; set; } = new(false);
        public DateTime AuthorizerForImmediateStartDate { get; set; }
        public ApproverBriefViewModel? Approver { get; set; } = new(false);
        public BTLBriefViewModel? RLTMember { get; set; } = new(false);
        public DateTime? RLTMemberApproveDate { get; set; }
        public BTLBriefViewModel? BTLApprover { get; set; } = new(false);
        public DateTime? BTLApproveDate { get; set; }
        public TELBriefViewModel? TELApprover { get; set; } = new(false);
        public DateTime? TELApprovalDate { get; set; }
        public MaintManagerBriefViewModel? MaintManager { get; set; } = new(false);
        public DateTime? MaintManagerApprovalDate { get; set; }

        public double TotalCost { get => FCOSections.Sum(x => x.Estimate); }
        public double TotalHours { get => FCOSections.Sum(x => x.DU); }
        public double TotalHeadCount { get => FCOSections.Sum(x => x.MN); }

        public List<FCOSectionModifyViewModel>? FCOLabourSections { get; set; } = new();
        public List<FCOSectionModifyViewModel>? FCOMaterialSections { get; set; } = new();
        public List<FCOSectionModifyViewModel>? FCOEquipmentSections { get; set; } = new();
        public List<FCOSectionModifyViewModel>? FCOShopSections { get; set; } = new();
        [BindNever]
        public List<FCOSectionModifyViewModel>? FCOSections { get => FCOLabourSections?.Concat(FCOMaterialSections?.Concat(FCOEquipmentSections?.Concat(FCOShopSections))).ToList() ?? new List<FCOSectionModifyViewModel>(); }
        public AttachmentModifyViewModel? Attachment { get; set; } = new();
        public double Total { get => FCOSections.Where(x => x.SectionType != FCOSectionCatalog.Shop).Sum(x => x.Estimate); }
        public double Contingency { get => Total / 10; }
        [Display(Name = "Total")]
        public double SubTotal { get => Total + Contingency; }
        public double TotalLabor { get { var laborEstimate = FCOSections.Where(x => x.SectionType == FCOSectionCatalog.Labour).Sum(x => x.Estimate); return laborEstimate + (laborEstimate / 10); } }
        public double TotalMaterial { get { var materialEstimate = FCOSections.Where(x => x.SectionType == FCOSectionCatalog.Material).Sum(x => x.Estimate); return materialEstimate + (materialEstimate / 10); } }
        public double TotalEquipment { get { var equipmentEstimate = FCOSections.Where(x => x.SectionType == FCOSectionCatalog.Equipment).Sum(x => x.Estimate); return equipmentEstimate + (equipmentEstimate / 10); } }
        public double TotalShop { get { var shopEstimate = FCOSections.Where(x => x.SectionType == FCOSectionCatalog.Shop).Sum(x => x.Estimate); return shopEstimate + (shopEstimate / 10); } }
        public double SectionTotal { get => TotalLabor + TotalMaterial + TotalEquipment + TotalShop; }

    }
}
