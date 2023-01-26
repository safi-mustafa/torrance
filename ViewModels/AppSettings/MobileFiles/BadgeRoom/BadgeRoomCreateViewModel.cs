using ViewModels.Shared;
using Enums;

namespace ViewModels.AppSettings.MobileFiles.BadgeRoom
{
    public class BadgeRoomCreateViewModel : BaseFileCreateViewModel, IBaseCrudViewModel
    {
        public override AttachmentEntityType FileType { get; set; } = AttachmentEntityType.BadgeRoom;
    }
}