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
        public override AttachmentEntityType Type { get; set; } = AttachmentEntityType.VehiclePass;

    }
}
