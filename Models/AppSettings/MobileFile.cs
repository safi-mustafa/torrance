using Enums;
using Helpers.Models.Shared;

namespace Models.AppSettings
{
    public class MobileFile : BaseDBModel
    {
        public string Name { get; set; }
        public string ExtensionType { get; set; }
        public string Url { get; set; }
        public DateTime UploadDate { get; set; }
        public AttachmentEntityType FileType { get; set; }
    }
}
