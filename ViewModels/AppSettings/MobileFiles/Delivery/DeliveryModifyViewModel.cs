﻿using Enums;
using Models.Common.Interfaces;
using ViewModels.Shared;

namespace ViewModels.AppSettings.MobileFiles.Delivery
{
    public class DeliveryModifyViewModel : BaseFileUpdateViewModel, IBaseCrudViewModel, IIdentitifier
    {
        public override AttachmentEntityType Type { get; set; } = AttachmentEntityType.Delivery;
    }
}
