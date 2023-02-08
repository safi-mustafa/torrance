using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using Helpers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repositories.Services.WeldRodRecordServices.ApproverService;
using Repositories.Services.WeldRodRecordServices.EmployeeService;
using System;
using ViewModels.Authentication;
using ViewModels.Authentication.Approver;
using ViewModels.DataTable;
using ViewModels.WeldingRodRecord.Employee;
using Web.Helpers;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Approver")]
    public class EmployeeController : CrudBaseController<EmployeeModifyViewModel, EmployeeModifyViewModel, EmployeeDetailViewModel, EmployeeDetailViewModel, EmployeeSearchViewModel>
    {
        private readonly IEmployeeService<EmployeeModifyViewModel, EmployeeModifyViewModel, EmployeeDetailViewModel> _employeeService;
        private readonly IApproverService<ApproverModifyViewModel, ApproverModifyViewModel, ApproverDetailViewModel> _approverService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeService<EmployeeModifyViewModel, EmployeeModifyViewModel, EmployeeDetailViewModel> employeeService, IApproverService<ApproverModifyViewModel, ApproverModifyViewModel, ApproverDetailViewModel> approverService, ILogger<EmployeeController> logger, IMapper mapper) : base(employeeService, logger, mapper, "Employee", "Accounts")
        {
            _employeeService = employeeService;
            _approverService = approverService;
            _logger = logger;
        }

        protected override void SetDatatableActions<T>(DatatablePaginatedResultModel<T> result)
        {
            if (User.IsInRole("Approver") || User.IsInRole("SuperAdmin"))
            {
                result.ActionsList = new List<DataTableActionViewModel>()
                {
                    new DataTableActionViewModel() {Action="ResetPassword",Title="ResetPassword",Href=$"/Employee/ResetPassword/Id"},
                };
            }
            if (User.IsInRole("SuperAdmin"))
            {
                result.ActionsList.AddRange(new List<DataTableActionViewModel>()
                {
                    new DataTableActionViewModel() {Action="Detail",Title="Detail",Href=$"/Employee/Detail/Id"},
                    new DataTableActionViewModel() {Action="Update",Title="Update",Href=$"/Employee/Update/Id"},
                    new DataTableActionViewModel() {Action="Delete",Title="Delete",Href=$"/Employee/Delete/Id"},
                });
            }

        }

        public async Task<ActionResult> ResetPassword(int id)
        {
            try
            {
                var response = await _employeeService.GetById(id);
                if (response.Status == System.Net.HttpStatusCode.OK)
                {
                    var parsedResponse = response as RepositoryResponseWithModel<EmployeeDetailViewModel>;
                    var model = parsedResponse?.ReturnModel ?? new();
                    ChangeAccessCodeVM viewModel = new ChangeAccessCodeVM
                    {
                        Id = model.Id ?? 0,
                        UserId = model.UserId,
                        CurrentAccessCode = model.EmployeeId
                    };
                    return View(viewModel);
                }
                else
                {
                    _logger.LogInformation($"Record with id " + id + "not found");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex) { _logger.LogError($"Account ResetPassword method threw an exception, Message: {ex.Message}"); return RedirectToAction("Index"); }
        }
        [HttpPost]
        public async Task<ActionResult> ResetPassword(ChangeAccessCodeVM model)
        {
            await _employeeService.ResetAccessCode(model);
            return RedirectToAction("Index");
        }
        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Full Name",data = "FirstName"},
                new DataTableViewModel{title = "Company",data = "Company.Name"},
                new DataTableViewModel{title = "Email",data = "Email"},
                 new DataTableViewModel{title = "Access Code",data = "EmployeeId"},
                //new DataTableViewModel{title = "Is Approver?",data = "IsApprover"},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-form-export"}

            };
        }
        public override async Task<ActionResult> Create(EmployeeModifyViewModel model)
        {
            bool isUnique = await _approverService.IsAccessCodeUnique(model.UserId, model.EmployeeId);
            if (!isUnique)
            {
                ModelState.AddModelError("EmployeeId", "Access Code already in use.");
            }
            return await base.Create(model);
        }
        public override async Task<ActionResult> Update(EmployeeModifyViewModel model)
        {
            bool isUnique = await _approverService.IsAccessCodeUnique(model.UserId, model.EmployeeId);
            if (!isUnique)
            {
                ModelState.AddModelError("EmployeeId", "Access Code already in use.");
            }
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            return await base.Update(model);
        }
        public async Task<IActionResult> ValidateEmployeeId(int id, string employeeId)
        {
            return Json(await _approverService.IsAccessCodeUnique(id, employeeId));
        }
        public async Task<IActionResult> ValidateEmployeeEmail(int id, string email)
        {
            return Json(await _employeeService.IsEmployeeEmailUnique(id, email));
        }
    }
}
