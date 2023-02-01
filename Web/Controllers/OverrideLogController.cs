﻿using AutoMapper;
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
                new DataTableViewModel{title = "Requester",data = "Requester"},
                new DataTableViewModel{title = "Date Submitted",data = "FormattedDateSubmitted"},
                new DataTableViewModel{title = "Work Scope",data = "WorkScope"},
                new DataTableViewModel{title = "Override Hours",data = "OverrideHours"},
                new DataTableViewModel{title = "PO Number",data = "PONumber"},
                new DataTableViewModel{title = "Contractor",data = "Contractor.Name"},
                new DataTableViewModel{title = "Shift",data = "Shift.Name"},
                new DataTableViewModel{title = "Reason for Request",data = "ReasonForRequest.Name"},
                new DataTableViewModel{title = "Craft Rate",data = "CraftRate.Name"},
                new DataTableViewModel{title = "Craft Skill",data = "CraftSkill.Name"},
                new DataTableViewModel{title = "Override Type",data = "OverrideType.Name"},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-form-export"}
            });
            return dataColumns;
        }

        public override Task<ActionResult> Create(ORLogModifyViewModel model)
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
            return View("~/Views/OverrideLog/_Index.cshtml", vm);
        }

    }

}
