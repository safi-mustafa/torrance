using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;
using Repositories.Shared.Interfaces;
using Repositories.Shared.UserInfoServices;
using ViewModels.CRUD;
using ViewModels.DataTable;
using ViewModels.OverrideLogs.ORLog;
using ViewModels.OverrideLogs;
using ViewModels.WeldingRodRecord.WRRLog;
using Repositories.Services.OverrideLogServices.ORLogService;

namespace Web.Controllers
{
    [Authorize]
    public class OverrideLogController : ApproveBaseController<IORLogService<ORLogModifyViewModel, ORLogModifyViewModel, ORLogDetailViewModel>, ORLogModifyViewModel, ORLogModifyViewModel, ORLogDetailViewModel, ORLogDetailViewModel, ORLogSearchViewModel>
    {
        private readonly IORLogService<ORLogModifyViewModel, ORLogModifyViewModel, ORLogDetailViewModel> _OverrideLogService;
        private readonly ILogger<OverrideLogController> _logger;
        private readonly IBaseApprove _baseApprove;

        public OverrideLogController(IORLogService<ORLogModifyViewModel, ORLogModifyViewModel, ORLogDetailViewModel> OverrideLogService, ILogger<OverrideLogController> logger, IMapper mapper, IUserInfoService userInfo) : base(OverrideLogService, logger, mapper, "OverrideLog", "Override Log", !(userInfo.LoggedInUserRoles().Contains("Admin") || userInfo.LoggedInUserRoles().Contains("SuperAdmin")))
        {
            _OverrideLogService = OverrideLogService;
            _logger = logger;
        }

        public override List<DataTableViewModel> GetColumns()
        {
            var dataColumns = new List<DataTableViewModel>();
            dataColumns.AddRange(new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Submitted",data = "FormattedDateSubmitted"},
                new DataTableViewModel{title = "Requester",data = "Requester.Name"},
                new DataTableViewModel{title = "Company",data = "Company.Name"},
                new DataTableViewModel{title = "Shift",data = "Shift.Name"},
                new DataTableViewModel{title = "Unit",data = "Unit.Name"},
                new DataTableViewModel{title = "Reason",data = "ReasonForRequest.Name"},
                new DataTableViewModel{title = "Override Type",data = "OverrideType.Name"},
                new DataTableViewModel{title = "Craft",data = "FormattedCraft"},
                new DataTableViewModel{title = "Override Hours",data = "OverrideHours"},
                new DataTableViewModel{title = "PO Number",data = "PONumber"},
                new DataTableViewModel{title = "Work Completed Date",data = "FormattedDateOfWorkCompleted"},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-form-export"}
            });
            return dataColumns;
        }

        protected override void SetDatatableActions<T>(DatatablePaginatedResultModel<T> result)
        {
            var actions = new List<DataTableActionViewModel>();
            if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin"))
            {
                actions.Add(new DataTableActionViewModel() { Action = "Update", Title = "Update", Href = $"/OverrideLog/Update/Id" });
                actions.Add(new DataTableActionViewModel() { Action = "Delete", Title = "Delete", Href = $"/OverrideLog/Delete/Id" });
            }
            actions.Add(new DataTableActionViewModel() { Action = "Detail", Title = "Detail", Href = $"/OverrideLog/Detail/Id" });
            result.ActionsList = actions;
        }

        public override Task<ActionResult> Create(ORLogModifyViewModel model)
        {
            if (User.IsInRole("Employee"))
            {
                ModelState.Remove("Requester.Id");
                ModelState.Remove("Requester.Name");
                ModelState.Remove("Approver.Name");
            }
            ModelState.Remove("Company.Name");
            return base.Create(model);
        }

        public override Task<ActionResult> Update(ORLogModifyViewModel model)
        {
            if (User.IsInRole("Employee"))
            {
                ModelState.Remove("Requester.Id");
                ModelState.Remove("Requester.Name");
                ModelState.Remove("Approver.Name");
            }
            ModelState.Remove("Company.Name");
            return base.Update(model);
        }

        public override ActionResult DataTableIndexView(CrudListViewModel vm)
        {
            return View("~/Views/OverrideLog/_Index.cshtml", vm);
        }

    }

}
