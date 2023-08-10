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

namespace ViewModels
{
    public class FCOLogModifyViewModel : BaseUpdateVM, IBaseCrudViewModel, IIdentitifier, IApprove
    {
        public Status Status { get; set; }
        [Display(Name = "Description of Finding (Attach marked-up P&ID or iso / Sketch)")]
        public string? DescriptionOfFinding { get; set; }
        [Display(Name = "Additional Information")]
        public string? AdditionalInformation { get; set; }
        [Display(Name = "Loop Identification # & Equipment Number")]
        public string? EquipmentNumber { get; set; }
        [Display(Name = "Service/Location")]
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
        public DateTime? Date { get; set; }

        public ContractorBriefViewModel Contractor { get; set; } = new();
        public CompanyBriefViewModel Company { get; set; } = new();
        public EmployeeBriefViewModel? Employee { get; set; } = new();
        public DepartmentBriefViewModel Department { get; set; } = new();
        public UnitBriefViewModel Unit { get; set; } = new();
        public FCOTypeBriefViewModel? FCOType { get; set; } = new();
        public FCOReasonBriefViewModel? FCOReason { get; set; } = new();
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

        public List<FCOSectionModifyViewModel>? FCOSections { get; set; } = new();
        public List<AttachmentModifyViewModel>? Attachments { get; set; }
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
