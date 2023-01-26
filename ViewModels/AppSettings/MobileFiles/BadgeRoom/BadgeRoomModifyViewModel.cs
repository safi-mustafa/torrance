﻿using Enums;
using Models.Common.Interfaces;
using ViewModels.Shared;

namespace ViewModels.AppSettings.MobileFiles.BadgeRoom
{
    public class BadgeRoomModifyViewModel : BaseFileUpdateViewModel, IBaseCrudViewModel, IIdentitifier
    {
        public override AttachmentEntityType FileType { get; set; } = AttachmentEntityType.BadgeRoom;
    }
}