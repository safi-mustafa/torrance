﻿using Enums;
using Helpers.Datetime;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Models.Common.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ViewModels.Authentication.User;
using ViewModels.Common.Company;
using ViewModels.Common.Contractor;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.Shared;
using ViewModels.TimeOnTools.TOTLog;
using ViewModels.WeldingRodRecord;
using ViewModels.WeldingRodRecord.Location;
using ViewModels.WeldingRodRecord.RodType;
using ViewModels.WeldingRodRecord.WeldMethod;

namespace ViewModels
{
    public class FCOLogDetailViewModel : LogCommonDetailViewModel, IApprove, ISrNo
    {
        public long Id { get; set; }
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
        public FCOTypeBriefViewModel? FCOType { get; set; } = new();
        public FCOReasonBriefViewModel? FCOReason { get; set; } = new();
        [Display(Name = "Designated Coordinator")]
        public DesignatedCoordinatorBriefViewModel? DesignatedCoordinator { get; set; }

        [Display(Name = "Authorized for Immediate Start")]
        public string? AuthorizerForImmediateStart => Approver.Name;
        [Display(Name = "Approved to Progress")]
        public string? ApprovedToProgress => Approver.Name;
        [Display(Name = "Endorsement – BTL")]
        public string? BTLApprover => Approver.Name;
        [Display(Name = "Endorsement – Unit Superintendent")]
        public string? EndorsmentUnitManager => Approver.Name;

        public double TotalCost { get; set; }
        public string TotalCostFormatted { get => string.Format("{0:C}", TotalCost); }
        public double TotalHours { get; set; }
        public double TotalHeadCount { get; set; }

        public List<FCOSectionModifyViewModel>? FCOLabourSections { get; set; } = new();
        public List<FCOSectionModifyViewModel>? FCOMaterialSections { get; set; } = new();
        public List<FCOSectionModifyViewModel>? FCOEquipmentSections { get; set; } = new();
        public List<FCOSectionModifyViewModel>? FCOShopSections { get; set; } = new();
        [BindNever]
        public List<FCOSectionModifyViewModel>? FCOSections { get; set; } = new();
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
