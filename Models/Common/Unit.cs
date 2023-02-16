using Enums;
using Helpers.Models.Shared;

namespace Models.Common
{
    public class Unit : BaseDBModel, IName
    {
        public string Name { get; set; }
        public string CostTrackerUnit { get; set; }
        public LogType Type { get; set; }
    }
}
