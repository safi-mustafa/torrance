using Enums;
using Helpers.Datetime;
using Models.Common.Interfaces;
using Helpers.Double;
using System.ComponentModel.DataAnnotations;
using ViewModels.Authentication.User;
using ViewModels.Common.Company;
using ViewModels.Common.Contractor;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.Shared;
using ViewModels.WeldingRodRecord;

namespace ViewModels
{
    public class FCOLogDetailViewModel : LogCommonDetailViewModel, IApprove, ISrNo
    {
        public long Id { get; set; }
        //public Status Status { get; set; }
        [Display(Name = "Description")]
        public string? DescriptionOfFinding { get; set; }
        [Display(Name = "Additional Information")]
        public string? AdditionalInformation { get; set; }
        [Display(Name = "Loop Identification # & Equipment Number")]
        public string? EquipmentNumber { get; set; }
        [Display(Name = "Service/Location")]
        public string? Location { get; set; }
        [Display(Name = "PreTA")]
        public bool PreTA { get; set; }
        [Display(Name = "Shutdown Required")]

        public bool ShutdownRequired { get; set; }
        [Display(Name = "Scaffold Required")]
        public bool ScaffoldRequired { get; set; }
        [Display(Name = "FCO No.")]
        public long SrNo { get; set; }
        [Display(Name = "FCO No.")]
        public string SrNoFormatted
        {
            get
            {
                var srNo = $"{Unit.CostTrackerUnit}-{SrNo.ToString().PadLeft(3, '0')}";
                return srNo;
            }
        }
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
        public DateTime Date { get; set; }
        [Display(Name = "Date")]
        public DateTime? ApprovalDate { get; set; }
        public string DateFormatted { get => Date.FormatDatetimeInPST(); }
        [Display(Name = "Date")]
        public string ApprovalDateFormatted { get => ApprovalDate?.FormatDatetimeInPST(); }
        public ContractorBriefViewModel Contractor { get; set; } = new();
        public CompanyBriefViewModel Company { get; set; } = new();
        public EmployeeBriefViewModel? Employee { get; set; } = new();
        public DepartmentBriefViewModel Department { get; set; } = new();
        public UnitBriefViewModel Unit { get; set; } = new();

        [Display(Name = "FCO Type")]
        public FCOTypeBriefViewModel? FCOType { get; set; } = new();

        [Display(Name = "Reason For Change")]
        public FCOReasonBriefViewModel? FCOReason { get; set; } = new();
        [Display(Name = "Designated Coordinator")]
        public DesignatedCoordinatorBriefViewModel? DesignatedCoordinator { get; set; }

        public AreaExecutionLeadBriefViewModel? AreaExecutionLead { get; set; } = new(false);
        [Display(Name = "Date")]
        public DateTime? AreaExecutionLeadApprovalDate { get; set; }
        public BusinessTeamLeaderBriefViewModel? BusinessTeamLeader { get; set; } = new(false);
        [Display(Name = "Date")]
        public DateTime? BusinessTeamLeaderApprovalDate { get; set; }
        public RejecterBriefViewModel? Rejecter { get; set; } = new(false);
        [Display(Name = "Date")]
        public DateTime? RejecterDate { get; set; }

        public double TotalCost { get; set; }
        public string TotalCostFormatted { get => string.Format("{0:C}", TotalCost); }
        public double TotalHours { get; set; }
        public double TotalHeadCount { get; set; }

        [Display(Name = "Material Name")]
        public string MaterialName { get; set; }
        [Display(Name = "Material Rate")]
        public double MaterialRate { get; set; }
        [Display(Name = "Equipment Name")]
        public string EquipmentName { get; set; }
        [Display(Name = "Equipment Rate")]
        public double EquipmentRate { get; set; }
        [Display(Name = "Shop Name")]
        public string ShopName { get; set; }
        [Display(Name = "Shop Rate")]
        public double ShopRate { get; set; }

        public string OtherDocumentionFormatted
        {
            get
            {
                var otherDocumentation = "";

                if (AnalysisOfAlternatives && EquipmentFailureReport)
                {
                    otherDocumentation = "Analysis of alternatives / Mitigation Options (Y/N), Equipment Failure Report";
                }
                else if (AnalysisOfAlternatives)
                {
                    otherDocumentation = "Analysis of alternatives / Mitigation Options (Y/N)";
                }
                else if (EquipmentFailureReport)
                {
                    otherDocumentation = "Equipment Failure Report";
                }
                return otherDocumentation;

            }
        }

        //public List<FCOSectionModifyViewModel>? FCOLabourSections { get; set; } = new();
        //public List<FCOSectionModifyViewModel>? FCOMaterialSections { get; set; } = new();
        //public List<FCOSectionModifyViewModel>? FCOEquipmentSections { get; set; } = new();
        //public List<FCOSectionModifyViewModel>? FCOShopSections { get; set; } = new();
        //[BindNever]
        public List<FCOSectionModifyViewModel>? FCOSections { get; set; } = new();
        public AttachmentModifyViewModel? Photo { get; set; } = new(AttachmentEntityType.FCOLogPhoto);
        public AttachmentModifyViewModel? File { get; set; } = new(AttachmentEntityType.FCOLogFile);
        public double Total { get => Math.Round((FCOSections.Where(x => x.SectionType == FCOSectionCatalog.Labour).Sum(x => x.Estimate) + MaterialRate + EquipmentRate), 2).FixNan(); }
        public double Contingency { get; set; }
        public double Contingencies
        {
            get
            {
                var cont = Math.Round((Total / Contingency), 2).FixNan();
                return cont;
            }
        }
        [Display(Name = "Total")]
        public double SubTotal { get => Math.Round((Total + Contingencies), 2).FixNan(); }
        public double TotalLabor
        {
            get
            {
                var laborEstimate = FCOSections.Where(x => x.SectionType == FCOSectionCatalog.Labour).Sum(x => x.Estimate);
                return Math.Round((laborEstimate + (laborEstimate / Contingency)), 2).FixNan();
            }
        }
        public double TotalMaterial
        {
            get
            {
                var materialEstimate = MaterialRate;
                return Math.Round((materialEstimate + (materialEstimate / Contingency)), 2).FixNan();
            }
        }
        public double TotalEquipment
        {
            get
            {
                var equipmentEstimate = EquipmentRate;
                return Math.Round((equipmentEstimate + (equipmentEstimate / Contingency)), 2).FixNan();
            }
        }
        public double TotalShop
        {
            get
            {
                var shopEstimate = ShopRate;
                return Math.Round((shopEstimate + (shopEstimate / Contingency)), 2).FixNan();
            }
        }
        public double SectionTotal
        {
            get => Math.Round((TotalLabor + TotalMaterial + TotalEquipment + TotalShop), 2).FixNan();
        }
        public List<FCOCommentsViewModel> FCOComments { get; set; } = new List<FCOCommentsViewModel>();

        public string FCOCommentsClass
        {
            get
            {
                return FCOComments.Count > 0 ? "colorRed" : "";
            }

        }
    }

    public class FCOCommentsViewModel
    {
        public string Comment { get; set; }
        public string CommentedBy { get; set; }
        public DateTime CommentedDate { get; set; }
    }
}
