using Enums;
using Pagination;
using ViewModels.Shared;

namespace ViewModels.AppSettings.MobileFiles.VehiclePass
{
    public class VehiclePassDetailViewModel : BaseFileDetailViewModel
    {
    }

    public class VehiclePassSearchViewModel : BaseFileSearchViewModel
    {
        public override AttachmentEntityType FileType { get; set; } = AttachmentEntityType.VehiclePass;

    }
}
