﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Models.Common.Interfaces;
using ViewModels.Shared;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.Authentication;
using ViewModels.Common.Contractor;
using ViewModels.TomeOnTools.PermitType;
using ViewModels.TomeOnTools.ReworkDelay;
using ViewModels.TomeOnTools.Shift;
using ViewModels.TomeOnTools.ShiftDelay;
using Enums;

namespace ViewModels.TomeOnTools.TOTLog
{
    public class TOTLogModifyViewModel : BaseUpdateVM, IBaseCrudViewModel, IIdentitifier
    {
        public DateTime Date { get; set; } = DateTime.Now;
        [Display(Name = "Twr", Prompt = "Add Twr")]
        public string Twr { get; set; }

        [Display(Name = "Man Hours", Prompt = "Add Man Hours")]
        [Range(1, long.MaxValue, ErrorMessage = "The Man Hours must be greater than zero.")]
        public long ManHours { get; set; }
        [Display(Name = "Start Of Work")]
        public DateTime StartOfWork { get; set; } = DateTime.Now;

        [Display(Name = "Delay Reason", Prompt = "Add Delay Reason")]
        public string DelayReason { get; set; }
        [Display(Name = "Job Description", Prompt = "Add Job Descriptions")]
        public string JobDescription { get; set; }
        [Display(Name = "Man Power", Prompt = "Add Man Power")]
        [Range(1, long.MaxValue, ErrorMessage = "The Man Power must be greater than zero.")]
        public long ManPower { get; set; }
        [Display(Name = "Equipment No", Prompt = "Add Equipment No")]
        [Range(1, long.MaxValue, ErrorMessage = "The Equipment No must be greater than zero.")]
        public long EquipmentNo { get; set; }
        [Display(Name = "Hours Delayed", Prompt = "Add Hours Delayed")]
        [Range(1, double.MaxValue, ErrorMessage = "The Hours Delayed must be greater than zero.")]
        public double HoursDelayed { get; set; }
        public ApproveStatus Status { get; set; }

        public DepartmentBriefViewModel Department { get; set; } = new DepartmentBriefViewModel();

        public UnitBriefViewModel Unit { get; set; } = new UnitBriefViewModel();

        public ContractorBriefViewModel Contractor { get; set; } = new ContractorBriefViewModel();

        public ShiftDelayBriefViewModel ShiftDelay { get; set; } = new ShiftDelayBriefViewModel();

        public ReworkDelayBriefViewModel ReworkDelay { get; set; } = new ReworkDelayBriefViewModel();

        public PermitTypeBriefViewModel PermitType { get; set; } = new PermitTypeBriefViewModel();

        public ShiftBriefViewModel Shift { get; set; } = new ShiftBriefViewModel();

        public UserBriefViewModel Approver { get; set; } = new UserBriefViewModel(true,"The Approver field is required.");

        public UserBriefViewModel Foreman { get; set; } = new UserBriefViewModel(true, "The Foreman field is required.");

    }
}