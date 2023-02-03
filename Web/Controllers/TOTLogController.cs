﻿using AutoMapper;
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

        protected override TOTLogSearchViewModel SetDefaultFilters()
        {
            var filters= base.SetDefaultFilters();
            filters.StatusNot = Enums.Status.Pending;
            return filters;
        }
        public override List<DataTableViewModel> GetColumns()
        {
            var dataColumns = new List<DataTableViewModel>();
            dataColumns.AddRange(new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Status",data = "FormattedStatus",format="html",formatValue="status"},
                new DataTableViewModel{title = "Date",data = "FormattedDate"},
                new DataTableViewModel{title = "Twr",data = "Twr"},
                new DataTableViewModel{title = "Man Hours",data = "ManHours"},
                new DataTableViewModel{title = "Start Of Work",data = "FormattedStartOfWork"},
                new DataTableViewModel{title = "Equipment No",data = "EquipmentNo"},
                new DataTableViewModel{title = "Hours Delayed",data = "HoursDelayed"},
                //new DataTableViewModel{title = "Status",data = "Status"},
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
