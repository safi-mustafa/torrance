using Helpers.Models.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Common
{
    public class ApproverAssociation : BaseDBModel
    {
        [ForeignKey("Approver")]
        public long ApproverId { get; set; }
        public ToranceUser Approver { get; set; }

        [ForeignKey("Unit")]
        public long UnitId { get; set; }
        public Unit Unit { get; set; }

        [ForeignKey("Department")]
        public long DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
