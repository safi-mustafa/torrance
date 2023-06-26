using Helpers.Models.Shared;
using Models.Common;

namespace Models.TimeOnTools
{
    public class OngoingWorkDelay : BaseDBModel, IName
    {
        public string Name { get; set; }
    }
}
