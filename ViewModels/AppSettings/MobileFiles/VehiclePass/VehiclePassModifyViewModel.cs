using Enums;
using Models.Common.Interfaces;
using ViewModels.Shared;

namespace ViewModels.AppSettings.MobileFiles.VehiclePass
{
    public class VehiclePassModifyViewModel : BaseFileUpdateViewModel, IBaseCrudViewModel, IIdentitifier
    {
        public override AttachmentEntityType FileType { get; set; } = AttachmentEntityType.VehiclePass;
    }
}
