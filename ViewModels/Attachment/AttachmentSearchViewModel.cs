using System;
using Pagination;
using ViewModels.Shared;

namespace ViewModels.Attachment
{
    public class AttachmentSearchViewModel : BaseSearchModel
    {
        public AttachmentSearchViewModel()
        {
        }
        public string Name { get; set; }
        public FolderBriefViewModel Folder { get; set; } = new();
    }
}

