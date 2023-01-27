using System.ComponentModel.DataAnnotations.Schema;
using Enums;

namespace Helpers.Models.Shared
{
    public class Attachment : BaseDBModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public long? EntityId { get; set; }
        public AttachmentEntityType EntityType { get; set; }

        [ForeignKey("Folder")]
        public long? FolderId { get; set; }
        public Folder? Folder { get; set; }
    }
}
