using Helpers.Models.Shared;
using Models.OverrideLogs;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Common
{
    public class CompanyCraft : BaseDBModel
    {
        [ForeignKey("Company")]
        public long CompanyId { get; set; }
        public Company Company { get; set; }

        [ForeignKey("CraftSkill")]
        public long CraftSkillId { get; set; }
        public CraftSkill CraftSkill { get; set; }
    }
}
