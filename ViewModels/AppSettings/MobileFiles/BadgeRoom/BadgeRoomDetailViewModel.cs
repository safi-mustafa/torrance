using Enums;
using Pagination;
using ViewModels.Shared;

namespace ViewModels.AppSettings.MobileFiles.BadgeRoom
{
    public class BadgeRoomDetailViewModel : BaseFileDetailViewModel
    {
    }

    public class BadgeRoomSearchViewModel : BaseFileSearchViewModel
    {
        public override AttachmentEntityType FileType { get; set; } = AttachmentEntityType.BadgeRoom;

    }
}
