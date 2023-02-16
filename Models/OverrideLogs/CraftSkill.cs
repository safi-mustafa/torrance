using Helpers.Models.Shared;
using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.OverrideLogs
{
    public class CraftSkill : BaseDBModel, IName
    {
        public string Name { get; set; }

        public double STRate { get; set; }
        public double OTRate { get; set; }
        public double DTRate { get; set; }
    }
}
