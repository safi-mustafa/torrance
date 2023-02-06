using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;
using Repositories.Services.TimeOnToolServices.TOTLogService;
using Repositories.Shared.Interfaces;
using Repositories.Shared.UserInfoServices;
using ViewModels.CRUD;
using ViewModels.DataTable;
using ViewModels.OverrideLogs.ORLog;
using ViewModels.TimeOnTools.TOTLog;
using ViewModels.WeldingRodRecord.WRRLog;

namespace Web.Controllers
{
    [Authorize]
    public class TOTLogController : ApproveBaseController<ITOTLogService<TOTLogModifyViewModel, TOTLogModifyViewModel, TOTLogDetailViewModel>, TOTLogModifyViewModel, TOTLogModifyViewModel, TOTLogDetailViewModel, TOTLogDetailViewModel, TOTLogSearchViewModel>
    {
        private readonly ITOTLogService<TOTLogModifyViewModel, TOTLogModifyViewModel, TOTLogDetailViewModel> _TOTLogService;
        private readonly ILogger<TOTLogController> _logger;
        private readonly IBaseApprove _baseApprove;

        public TOTLogController(ITOTLogService<TOTLogModifyViewModel, TOTLogModifyViewModel, TOTLogDetailViewModel> TOTLogService, ILogger<TOTLogController> logger, IMapper mapper, IUserInfoService userInfo) : base(TOTLogService, logger, mapper, "TOTLog", "Time On Tool Logs", !(userInfo.LoggedInUserRoles().Contains("Admin") || userInfo.LoggedInUserRoles().Contains("SuperAdmin")))
        {
            _TOTLogService = TOTLogService;
            _logger = logger;
        }

        protected override CrudListViewModel OverrideCrudListVM(CrudListViewModel vm)
        {
            vm.DataTableHeaderHtml = @"
                    <div class=""p-2 row"">
                        <span class=""badge Approved m-1""> </span>
                        <span class=""stat-name"">Approved</span>
                    </div>
                    <div class=""m-2 row"">
                        <span class=""badge Rejected m-1""> </span>
                        <span class=""stat-name"">Rejected</span>
                    </div>";
            vm.IsResponsiveDatatable = false;
            return vm;
        }

        protected override TOTLogSearchViewModel SetDefaultFilters()
        {
            var filters = base.SetDefaultFilters();
            filters.StatusNot = Enums.Status.Pending;
            return filters;
        }
        public override List<DataTableViewModel> GetColumns()
        {
            var dataColumns = new List<DataTableViewModel>();
            dataColumns.AddRange(new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Status",data = "FormattedStatus",format="html",formatValue="status"},
                new DataTableViewModel{title = "Submitted",data = "FormattedCreatedOn"},
                new DataTableViewModel{title = "Requester",data = "Employee.Name"},
                new DataTableViewModel{title = "Company",data = "Company.Name"},
                new DataTableViewModel{title = "Shift",data = "Shift.Name"},
                new DataTableViewModel{title = "Unit",data = "Unit.Name"},
                new DataTableViewModel{title = "Twr",data = "Twr"},
                new DataTableViewModel{title = "Equipment No",data = "EquipmentNo"},
                new DataTableViewModel{title = "Delay Type",data = "DelayType.Name"},
                new DataTableViewModel{title = "Reason",data = "ReasonForRequest.Name"},
                new DataTableViewModel{title = "Permit Type",data = "PermitType.Name"},
                new DataTableViewModel{title = "Start Date",data = "FormattedStartOfWork"},
                new DataTableViewModel{title = "Total Head Count",data = "ManPowerAffected"},
                new DataTableViewModel{title = "Total Manhours",data = "ManHours"},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-form-export"}
            });
            return dataColumns;
        }

        public override Task<ActionResult> Create(TOTLogModifyViewModel model)
        {
            if (User.IsInRole("Employee"))
            {
                ModelState.Remove("Employee.Id");
                ModelState.Remove("Employee.Name");
            }
            return base.Create(model);
        }

        public override Task<ActionResult> Update(int id)
        {
            if (User.IsInRole("Employee"))
            {
                ModelState.Remove("Employee.Id");
                ModelState.Remove("Employee.Name");
            }
            return base.Update(id);
        }

        public override ActionResult DataTableIndexView(CrudListViewModel vm)
        {
            return View("~/Views/TOTLog/_Index.cshtml", vm);
        }

        protected override void SetDatatableActions<T>(DatatablePaginatedResultModel<T> result)
        {
            result.ActionsList = new List<DataTableActionViewModel>()
            {
                    new DataTableActionViewModel() {Action="Detail",Title="Detail",Href=$"/TOTLog/Detail/Id"},
            };
        }


    }

}
