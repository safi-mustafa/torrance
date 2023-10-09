using System;
using Enums;
using Helpers.Datetime;
using Helpers.Extensions;
using Models;
using Models.Common.Interfaces;
using Models.OverrideLogs;
using Models.TimeOnTools;
using Models.WeldingRodRecord;
using ViewModels.Shared;

namespace ViewModels.Common
{
    public class ApprovalDetailViewModel : IBaseCrudViewModel, IIdentitifier
    {
        public ApprovalDetailViewModel()
        {
        }
        public long Id { get; set; }
        public Status Status { get; set; }
        public string FormattedStatus { get => Status.GetDisplayName(); }
        public string FormattedStatusForView { get => Status.ToString(); }

        public DateTime Date { get; set; }
        public string FormattedDate { get => Date.FormatDatetimeInPST(); }
        public LogType Type { get; set; }
        public string FormattedLogType { get => Type.GetDisplayName(); }
        public ToranceUser? Employee { get; set; }
        public string Requester { get => Employee != null ? $"{Employee.FullName}" : ""; }
        public string Department { get; set; }
        public string Contractor { get; set; }
        public DelayType DelayType { get; set; }
        public string ResonForDelay { get => DelayType != null ? DelayType.Name : ""; }

        public string Reason { get; set; }

        public double TotalCost { get; set; }

        public string Approver { get; set; }
        public string Unit { get; set; }
        public string TWR { get; set; }
        public double TotalHours { get; set; }

        public double TotalHeadCount { get; set; }
        public ActiveStatus ActiveStatus { get; set; }
    }
}

