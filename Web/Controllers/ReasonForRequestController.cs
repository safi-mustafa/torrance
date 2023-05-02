using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.OverrideLogServices.ReasonForRequestService;
using ViewModels.DataTable;
using ViewModels.OverrideLogs;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    public class ReasonForRequestController : CrudBaseController<ReasonForRequestModifyViewModel, ReasonForRequestModifyViewModel, ReasonForRequestDetailViewModel, ReasonForRequestDetailViewModel, ReasonForRequestSearchViewModel>
    {
        private readonly IReasonForRequestService<ReasonForRequestModifyViewModel, ReasonForRequestModifyViewModel, ReasonForRequestDetailViewModel> _reasonForRequestService;
        private readonly ILogger<ReasonForRequestController> _logger;

        public ReasonForRequestController(IReasonForRequestService<ReasonForRequestModifyViewModel, ReasonForRequestModifyViewModel, ReasonForRequestDetailViewModel> reasonForRequestService, ILogger<ReasonForRequestController> logger, IMapper mapper) : base(reasonForRequestService, logger, mapper, "ReasonForRequest", "Reason For Requests")
        {
            _reasonForRequestService = reasonForRequestService;
            _logger = logger;
        }

        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Name",data = "Name", orderable = true},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-from-export"}

            };
        }
    }
}
