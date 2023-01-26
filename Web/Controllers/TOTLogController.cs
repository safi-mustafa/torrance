﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Services.TimeOnToolServices.TOTLogService;
using Repositories.Shared.Interfaces;
using ViewModels.CRUD;
using ViewModels.DataTable;
using ViewModels.TomeOnTools.TOTLog;

namespace Web.Controllers
{
    [Authorize]
    public class TOTLogController : ApproveBaseController<ITOTLogService<TOTLogModifyViewModel, TOTLogModifyViewModel, TOTLogDetailViewModel>, TOTLogModifyViewModel, TOTLogModifyViewModel, TOTLogDetailViewModel, TOTLogDetailViewModel, TOTLogSearchViewModel>
    {
        private readonly ITOTLogService<TOTLogModifyViewModel, TOTLogModifyViewModel, TOTLogDetailViewModel> _TOTLogService;
        private readonly ILogger<TOTLogController> _logger;
        private readonly IBaseApprove _baseApprove;

        public TOTLogController(ITOTLogService<TOTLogModifyViewModel, TOTLogModifyViewModel, TOTLogDetailViewModel> TOTLogService, ILogger<TOTLogController> logger, IMapper mapper) : base(TOTLogService, logger, mapper, "TOTLog", "Time On Tool Logs")
        {
            _TOTLogService = TOTLogService;
            _logger = logger;
        }

        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "<input type='checkbox' id='master-checkbox'>", data = "Id", format = "html", formatValue = "checkbox",className="exclude-form-export" },
                new DataTableViewModel{title = "Date",data = "FormattedDate"},
                new DataTableViewModel{title = "Twr",data = "Twr"},
                new DataTableViewModel{title = "Man Hours",data = "ManHours"},
                new DataTableViewModel{title = "Start Of Work",data = "FormattedStartOfWork"},
                new DataTableViewModel{title = "Equipment No",data = "EquipmentNo"},
                new DataTableViewModel{title = "Hours Delayed",data = "HoursDelayed"},
                new DataTableViewModel{title = "Status",data = "Status"},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-form-export"}
            };
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

        public override ActionResult DataTableIndexView(CrudListViewModel vm)
        {
            return View("~/Views/TOTLog/_Index.cshtml", vm);
        }




    }
     
}
