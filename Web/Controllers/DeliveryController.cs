using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using ViewModels.DataTable;
using Repositories.Services.AppSettingServices.MobileFileServices;
using ViewModels.AppSettings.MobileFiles.Delivery;

namespace Web.Controllers
{
    [Authorize]
    
    public class DeliveryController : CrudBaseController<DeliveryModifyViewModel, DeliveryModifyViewModel, DeliveryDetailViewModel, DeliveryDetailViewModel, DeliverySearchViewModel>
    {
        private readonly IMobileFileService<DeliveryModifyViewModel, DeliveryModifyViewModel, DeliveryDetailViewModel> _deliveryService;
        private readonly ILogger<DeliveryController> _logger;

        public DeliveryController(IMobileFileService<DeliveryModifyViewModel, DeliveryModifyViewModel, DeliveryDetailViewModel> deliveryService, ILogger<DeliveryController> logger, IMapper mapper) : base(deliveryService, logger, mapper, "Delivery", "Delivery")
        {
            _deliveryService = deliveryService;
            _logger = logger;
        }

        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Url",data = "Url"},
                new DataTableViewModel{title = "Status",data = "FormattedStatus"},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-form-export"}

            };
        }
    }
}
