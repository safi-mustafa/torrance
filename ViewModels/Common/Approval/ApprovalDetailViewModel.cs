using System;
using Enums;
using Helpers.Extensions;
using Models.Common.Interfaces;
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
        public DateTime Date { get; set; }
        public string FormattedDate { get => Date.ToString("MM/dd/yyyy HH:mm:ss"); }
        public LogType Type { get; set; }
        public string FormattedLogType { get => Type.GetDisplayName(); }
        public Employee? Employee { get; set; }
        public string Requester { get => Employee != null ? $"{Employee.FirstName} {Employee.LastName}" : ""; }
        public string Department { get; set; }
        public string Contractor { get; set; }

        public string Approver { get; set; }
        public string Unit { get; set; }
        public string TWR { get; set; }
        public ActiveStatus ActiveStatus { get; set; }
    }
}

