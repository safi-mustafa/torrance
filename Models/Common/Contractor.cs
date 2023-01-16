using Enums;
using Helpers.Models.Shared;

namespace Models.Common
{
    public class Contractor : BaseDBModel
    {
        public string Name { get; set; }
        public LogType Type { get; set; }
    }
}
