using Microsoft.AspNetCore.Http;

namespace Helpers.Models.Shared
{
    public class Folder : BaseDBModel
    {
        public string Name { get; set; }
        public string? IconUrl { get; set; }
    }
}
