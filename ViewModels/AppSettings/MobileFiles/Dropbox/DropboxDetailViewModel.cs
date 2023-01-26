﻿using Enums;
using Pagination;
using ViewModels.Shared;

namespace ViewModels.AppSettings.MobileFiles.Dropbox
{
    public class DropboxDetailViewModel : BaseFileDetailViewModel
    {

    }

    public class DropboxSearchViewModel : BaseFileSearchViewModel
    {
        public override AttachmentEntityType FileType { get; set; } = AttachmentEntityType.Dropbox;

        public override ActiveStatus ActiveStatus { get; set; } = ActiveStatus.Active;

    }
}