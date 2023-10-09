using Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Authentication.User;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.Notification;

namespace ViewModels.WeldingRodRecord.WRRLog
{
    public interface IWRRLogNotificationViewModel
    {
        string Twr { get; }
        EmployeeBriefViewModel Employee { get; set; }
        UnitBriefViewModel Unit { get; set; }
        DepartmentBriefViewModel Department { get; set; }

        ApproverBriefViewModel Approver { get; set; }

    }
    public class WRRLogNotificationViewModel : INotificationMetaViewModel
    {
        public WRRLogNotificationViewModel(IWRRLogNotificationViewModel model, long id)
        {
            LogId = id;
            DepartmentId = model.Department?.Id?.ToString();
            UnitId = model.Unit?.Id?.ToString();
            IdentifierKey = "TWR#";
            IdentifierValue = model.Twr;
            RequestorId = model.Employee.Id;
            ApproverId = model.Approver?.Id?.ToString();
            EntityType = NotificationEntityType.WRRLog;
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
