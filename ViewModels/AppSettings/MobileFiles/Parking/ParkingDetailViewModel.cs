﻿using Enums;
using Pagination;
using ViewModels.Shared;

namespace ViewModels.AppSettings.MobileFiles.Parking
{
    public class ParkingDetailViewModel : BaseFileDetailViewModel
    {
    }

    public class ParkingSearchViewModel : BaseFileSearchViewModel
    {
        public override AttachmentEntityType FileType { get; set; } = AttachmentEntityType.Parking;

    }
}