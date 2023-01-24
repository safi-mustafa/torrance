using Enums;
using Models.Common.Interfaces;

namespace ViewModels.Shared
{
    public class BadgeRoomModifyViewModel : BaseFileUpdateViewModel, IBaseCrudViewModel, IIdentitifier
    {
        public override AttachmentEntityType Type { get; set; } = AttachmentEntityType.BadgeRoom;
    }
}
