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

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator,Approver")]
    public class EmployeeController : UserController<EmployeeModifyViewModel, EmployeeModifyViewModel, EmployeeDetailViewModel, EmployeeDetailViewModel, EmployeeSearchViewModel>
    {
        private readonly IEmployeeService<EmployeeModifyViewModel, EmployeeModifyViewModel, EmployeeDetailViewModel> _employeeService;
        private readonly IApproverService<ApproverModifyViewModel, ApproverModifyViewModel, ApproverDetailViewModel> _approverService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeService<EmployeeModifyViewModel, EmployeeModifyViewModel, EmployeeDetailViewModel> employeeService, IApproverService<ApproverModifyViewModel, ApproverModifyViewModel, ApproverDetailViewModel> approverService, ILogger<EmployeeController> logger, IMapper mapper) : base(employeeService, logger, mapper, "Employee", "Requestor", RolesCatalog.Employee)
        {
            _employeeService = employeeService;
            _approverService = approverService;
            _logger = logger;
        }


        public IActionResult ImportExcelSheet()
        {
            var model = new ExcelFileVM();
            return View(model);
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
