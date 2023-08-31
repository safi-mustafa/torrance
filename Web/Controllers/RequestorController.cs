using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using Enums;
using Helpers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repositories.Services.AppSettingServices.EmployeeService;
using Repositories.Services.AppSettingServices.ApproverService;
using System;
using ViewModels.Authentication;
using ViewModels.Authentication.Approver;
using ViewModels.DataTable;
using ViewModels.WeldingRodRecord.Employee;
using Web.Helpers;
using ViewModels.Shared;
using Pagination;
using Select2;
using Newtonsoft.Json;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator,Approver")]
    public class RequestorController : UserController<EmployeeModifyViewModel, EmployeeModifyViewModel, EmployeeDetailViewModel, EmployeeDetailViewModel, EmployeeSearchViewModel>
    {
        private readonly IEmployeeService<EmployeeModifyViewModel, EmployeeModifyViewModel, EmployeeDetailViewModel> _employeeService;
        private readonly IApproverService<ApproverModifyViewModel, ApproverModifyViewModel, ApproverDetailViewModel> _approverService;
        private readonly ILogger<RequestorController> _logger;
        private readonly UserManager<ToranceUser> _userManager;

        public RequestorController(IEmployeeService<EmployeeModifyViewModel, EmployeeModifyViewModel, EmployeeDetailViewModel> employeeService, IApproverService<ApproverModifyViewModel, ApproverModifyViewModel, ApproverDetailViewModel> approverService, ILogger<RequestorController> logger, IMapper mapper, UserManager<ToranceUser> userManager) : base(employeeService, logger, mapper, userManager, "Requestor", "Requestor", RolesCatalog.Employee)
        {
            _employeeService = employeeService;
            _approverService = approverService ?? throw new ArgumentNullException(nameof(approverService));
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult ImportExcelSheet()
        {
            var model = new ExcelFileVM();
            return View(model);
        }

        public override Task<ActionResult> Create(EmployeeModifyViewModel model)
        {
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            if (string.IsNullOrEmpty(model.Password))
            {
                model.Password = "Torrance";
            }
            return base.Create(model);
        }

        [HttpPost]
        public async Task<ActionResult> ImportExcelSheet(ExcelFileVM model)
        {
            if (await _employeeService.InitializeExcelContractData(model))
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("ImportExcelSheet");
        }
    }
}
