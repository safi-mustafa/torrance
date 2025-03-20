using Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Common.Approval
{
    public class ApprovalModifyViewModel
    {
        public long Id { get; set; }
        public Status Status { get; set; }
        public string? Comment { get; set; }
        public bool IsUnauthenticatedApproval { get; set; } = false;
        public long ApproverId { get; set; } = 0;
        public Guid NotificationId { get; set; } = new Guid();
    }
}
