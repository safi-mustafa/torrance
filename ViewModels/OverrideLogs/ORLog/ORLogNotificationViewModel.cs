using Enums;
using ViewModels.Authentication.User;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.Notification;
using ViewModels.WeldingRodRecord;

namespace ViewModels.OverrideLogs.ORLog
{
    public interface IORLogNotificationViewModel
    {
        long PoNumber { get; set; }
        EmployeeBriefViewModel Employee { get; set; }
        UnitBriefViewModel Unit { get; set; }
        DepartmentBriefViewModel Department { get; set; }

        ApproverBriefViewModel Approver { get; set; }

    }
    public class ORLogNotificationViewModel : INotificationMetaViewModel
    {
        public ORLogNotificationViewModel(IORLogNotificationViewModel model, long id)
        {
            LogId = id;
            DepartmentId = model.Department?.Id?.ToString();
            UnitId = model.Unit?.Id?.ToString();
            IdentifierKey = "PO#";
            IdentifierValue = model.PoNumber.ToString();
            RequestorId = model.Employee.Id;
            ApproverId = model.Approver?.Id?.ToString();
            EntityType = NotificationEntityType.OverrideLog;
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
