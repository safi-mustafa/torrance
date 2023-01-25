using Enums;
using Models.Common.Interfaces;
using ViewModels.Shared;

namespace ViewModels.AppSettings.MobileFiles.Passport
{
    public class PassportModifyViewModel : BaseFileUpdateViewModel, IBaseCrudViewModel, IIdentitifier
    {
        public override AttachmentEntityType FileType { get; set; } = AttachmentEntityType.Passport;
    }
}
