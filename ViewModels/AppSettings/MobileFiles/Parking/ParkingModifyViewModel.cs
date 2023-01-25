using Enums;
using Models.Common.Interfaces;
using ViewModels.Shared;

namespace ViewModels.AppSettings.MobileFiles.Parking
{
    public class ParkingModifyViewModel : BaseFileUpdateViewModel, IBaseCrudViewModel, IIdentitifier
    {
        public override AttachmentEntityType FileType { get; set; } = AttachmentEntityType.Parking;
    }
}
