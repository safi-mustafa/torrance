﻿using Enums;
using Pagination;
using ViewModels.Shared;

namespace ViewModels.AppSettings.MobileFiles.Delivery
{
    public class DeliveryDetailViewModel : BaseFileDetailViewModel
    {
    }

    public class DeliverySearchViewModel : BaseFileSearchViewModel
    {
        public override AttachmentEntityType FileType { get; set; } = AttachmentEntityType.Delivery;

    }
}