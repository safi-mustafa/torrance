using Helpers.Models.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Common
{
    public class ApproverUnit : BaseDBModel
    {
        [ForeignKey("TorranceUser")]
        public long ApproverId { get; set; }
        public ToranceUser Approver { get; set; }

        [ForeignKey("Unit")]
        public long UnitId { get; set; }
        public Unit Unit { get; set; }
    }
}
