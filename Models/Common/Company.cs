using Helpers.Models.Shared;

namespace Models.Common
{
    public class Company : BaseDBModel, IName
    {
        public string Name { get; set; }
    }
}
