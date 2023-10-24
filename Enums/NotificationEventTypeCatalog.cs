using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Enums
{
    public enum NotificationEventTypeCatalog
    {
        Created,
        Updated,
        [Display(Name = "Approver Assigned")]
        ApproverAssigned,
        Approved,
        Rejected
    }
}

