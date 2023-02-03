﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Models.Common.Interfaces;
using ViewModels.Shared;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.Authentication;
using ViewModels.Common.Contractor;
using ViewModels.TimeOnTools.PermitType;
using ViewModels.TimeOnTools.ReworkDelay;
using ViewModels.TimeOnTools.Shift;
using ViewModels.TimeOnTools.ShiftDelay;
using Enums;
using ViewModels.AppSettings.Map;
using ViewModels.TimeOnTools.PermittingIssue;
using ViewModels.WeldingRodRecord;

namespace ViewModels.TimeOnTools.TOTLog
{
    public class TOTLogModifyViewModel : BaseUpdateVM, IBaseCrudViewModel, IIdentitifier
    {
        public DateTime? Date { get; set; } = DateTime.Now;
        [Display(Name = "Twr", Prompt = "Add Twr")]
        public string Twr { get; set; }

        [Display(Name = "Man Hours", Prompt = "Add Man Hours")]
        [Range(1, long.MaxValue, ErrorMessage = "The Man Hours must be greater than zero.")]
        public long ManHours { get; set; }
        [Display(Name = "Start Of Work")]
        public DateTime StartOfWork { get; set; } = DateTime.Now;
        [Display(Name = "Time Requested")]
        public TimeSpan? TimeRequested { get; set; } = TimeSpan.Zero;
        [Display(Name = "Time Signed")]
        public TimeSpan? TimeSigned { get; set; } = TimeSpan.Zero;
        public string? Comment { get; set; }

        [Display(Name = "Delay Reason", Prompt = "Add Delay Reason")]
        public string DelayReason { get; set; }
        [Display(Name = "Job Description", Prompt = "Add Job Descriptions")]
        public string? JobDescription { get; set; }
        [Display(Name = "Man Power", Prompt = "Add Man Power")]
        [Range(1, long.MaxValue, ErrorMessage = "The Man Power must be greater than zero.")]
        public long ManPowerAffected { get; set; }
        [Display(Name = "Equipment No", Prompt = "Add Equipment No")]
        [Range(1, long.MaxValue, ErrorMessage = "The Equipment No must be greater than zero.")]
        public long EquipmentNo { get; set; }
        [Display(Name = "Hours Delayed", Prompt = "Add Hours Delayed")]
        //[Range(1, double.MaxValue, ErrorMessage = "The Hours Delayed must be greater than zero.")]
        public double? HoursDelayed { get; set; }
        public Status Status { get; set; }

        public DepartmentBriefViewModel Department { get; set; } = new DepartmentBriefViewModel();

        public UnitBriefViewModel Unit { get; set; } = new UnitBriefViewModel();

        public ContractorBriefViewModel Contractor { get; set; } = new ContractorBriefViewModel();

        public ShiftDelayBriefViewModel ShiftDelay { get; set; } = new ShiftDelayBriefViewModel();

        public ReworkDelayBriefViewModel ReworkDelay { get; set; } = new ReworkDelayBriefViewModel();

        public PermitTypeBriefViewModel PermitType { get; set; } = new PermitTypeBriefViewModel();

        public ShiftBriefViewModel Shift { get; set; } = new ShiftBriefViewModel();
        public PermittingIssueBriefViewModel PermittingIssue { get; set; } = new PermittingIssueBriefViewModel();

        private ApproverBriefViewModel? _approver;
        public ApproverBriefViewModel Approver { get => _approver == null ? new ApproverBriefViewModel() : _approver; set => _approver = value; }

        private UserBriefViewModel? _foreman;
        public UserBriefViewModel Foreman { get => _foreman == null ? new UserBriefViewModel() : _foreman; set => _foreman = value; }

        private EmployeeBriefViewModel? _employee;
        public EmployeeBriefViewModel Employee { get => _employee == null ? new EmployeeBriefViewModel() : _employee; set => _employee = value; }
    }
}