using Enums;
using Models.Common.Interfaces;
using ViewModels.Shared;

namespace ViewModels.AppSettings.MobileFiles.Dropbox
{
    public class DropboxModifyViewModel : BaseUpdateVM, IBaseCrudViewModel, IIdentitifier
    {
        public string? Url { get; set; }
    }
}
