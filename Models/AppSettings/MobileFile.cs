using Enums;
using Helpers.Models.Shared;

namespace Models.AppSettings
{
    public class MobileFile : BaseDBModel
    {
        public string Url { get; set; }
        public AttachmentEntityType Type { get; set; }
    }
}
