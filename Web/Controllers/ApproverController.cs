using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pagination;
using Repositories.Services.AppSettingServices.ApproverService;
using Select2.Model;
using ViewModels.Authentication;
using ViewModels.Authentication.Approver;
using ViewModels.Authentication.User;
using ViewModels.DataTable;
using Enums;
using ViewModels.OverrideLogs.ORLog;
using Microsoft.AspNetCore.Identity;
using Models;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator,Admin")]
    public class ApproverController : UserController<ApproverModifyViewModel, ApproverModifyViewModel, ApproverDetailViewModel, ApproverDetailViewModel, ApproverSearchViewModel>
    {
        private readonly IApproverService<ApproverModifyViewModel, ApproverModifyViewModel, ApproverDetailViewModel> _service;
        private readonly ILogger<ApproverController> _logger;
        private readonly UserManager<ToranceUser> _userManager;

        public ApproverController(IApproverService<ApproverModifyViewModel, ApproverModifyViewModel, ApproverDetailViewModel> service, ILogger<ApproverController> logger, IMapper mapper, UserManager<ToranceUser> userManager) : base(service, logger, mapper, userManager, "Approver", "Approver", RolesCatalog.Approver)
        {
            _service = service;
            _logger = logger;
            _userManager = userManager;
        }

        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Email",data = "Email",orderable=true},
                new DataTableViewModel{title = "Full Name",data = "FullName", orderable = true},
                new DataTableViewModel{title = "Access Code",data = "FormattedAccessCode", orderable = false},
                new DataTableViewModel{title = "Can Add Logs?",data = "FormattedCanAddLogs", orderable = false},
                new DataTableViewModel{title = "Status",data = "FormattedStatus", orderable = false},
                //new DataTableViewModel{title = "Units",data = "FormattedUnits"},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-from-export"}

            };
        }

        public IActionResult _ApproverAssociationRow(ApproverAssociationsViewModel model, int rowNumber)
        {
            ViewData["RowNumber"] = rowNumber;
            return PartialView("_ApproverAssociationRow", model);
        }

    }
}
