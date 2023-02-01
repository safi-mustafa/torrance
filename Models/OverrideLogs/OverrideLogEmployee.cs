using Helpers.Models.Shared;
using Models.WeldingRodRecord;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.OverrideLogs
{
    public class OverrideLogEmployee : BaseDBModel
    {
        [ForeignKey("Employee")]
        public long EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [ForeignKey("OverrideLog")]
        public long OverrideLogId { get; set; }
        public OverrideLog OverrideLog { get; set; }
    }
}
