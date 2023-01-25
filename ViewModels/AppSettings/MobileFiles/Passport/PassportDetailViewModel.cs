using Enums;
using Pagination;
using ViewModels.Shared;

namespace ViewModels.AppSettings.MobileFiles.Passport
{
    public class PassportDetailViewModel : BaseFileDetailViewModel
    {
    }

    public class PassportSearchViewModel : BaseFileSearchViewModel
    {
        public override AttachmentEntityType FileType { get; set; } = AttachmentEntityType.Passport;

    }
}
