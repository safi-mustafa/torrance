using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using ViewModels.DataTable;
using Repositories.Services.AppSettingServices.MobileFileServices;
using ViewModels.AppSettings.MobileFiles.Passport;

namespace Web.Controllers
{
    [Authorize]
    
    public class PassportController : CrudBaseController<PassportModifyViewModel, PassportModifyViewModel, PassportDetailViewModel, PassportDetailViewModel, PassportSearchViewModel>
    {
        private readonly IMobileFileService<PassportModifyViewModel, PassportModifyViewModel, PassportDetailViewModel> _deliveryService;
        private readonly ILogger<PassportController> _logger;

        public PassportController(IMobileFileService<PassportModifyViewModel, PassportModifyViewModel, PassportDetailViewModel> deliveryService, ILogger<PassportController> logger, IMapper mapper) : base(deliveryService, logger, mapper, "Passport", "Passport")
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
