using ViewModels.Shared;

namespace ViewModels.Attachment
{
    public class AttachmentModifyViewModel : AttachmentVM
    {
        public AttachmentModifyViewModel()
        {
        }
        public FolderBriefViewModel Folder { get; set; }
    }
}

