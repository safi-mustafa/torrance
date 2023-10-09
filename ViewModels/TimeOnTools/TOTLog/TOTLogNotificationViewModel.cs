using Enums;
using ViewModels.Authentication.User;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.Notification;
using ViewModels.WeldingRodRecord;

namespace ViewModels.TimeOnTools.TOTLog
{
    public interface ITOTLogNotificationViewModel
    {
        string? PermitNo { get; set; }
        EmployeeBriefViewModel Employee { get; set; }
        UnitBriefViewModel Unit { get; set; }
        DepartmentBriefViewModel Department { get; set; }
        ApproverBriefViewModel Approver { get; set; }

    }
    public class TOTLogNotificationViewModel : INotificationMetaViewModel
    {
        public TOTLogNotificationViewModel(ITOTLogNotificationViewModel model,long id)
        {
            LogId = id;
            DepartmentId = model.Department?.Id?.ToString();
            UnitId = model.Unit?.Id?.ToString();
            IdentifierKey = "Permit#";
            IdentifierValue = model.PermitNo.ToString();
            RequestorId = model.Employee.Id;
            ApproverId = model.Approver?.Id?.ToString();
            EntityType = NotificationEntityType.TOTLog;
        }

        public long LogId { get; set; }
        public string Department { get; set; }
        public string DepartmentId { get; set; }
        public string Unit { get; set; }
        public string UnitId { get; set; }
        public long? RequestorId { get; set; }

        public string Requestor { get; set; }
        public string IdentifierKey { get; set; }
        public string IdentifierValue { get; set; }

        public NotificationEntityType EntityType { get; set; }

        public string ApproverId { get; set; }
        public string Approver { get; set; }
    }
}
