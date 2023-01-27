using Enums;
using Pagination;
using ViewModels.Shared;

namespace ViewModels.AppSettings.MobileFiles.Dropbox
{
    public class DropboxDetailViewModel : BaseCrudViewModel
    {
        public long Id { get; set; }
        public string? Url { get; set; }
    }

    public class DropboxSearchViewModel : BaseSearchModel
    {
        public ActiveStatus? ActiveStatus { get; set; } = null;
    }
}
