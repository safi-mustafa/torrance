using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using Helpers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repositories.Services.WeldRodRecordServices.EmployeeService;
using System;
using ViewModels.Authentication;
using ViewModels.DataTable;
using ViewModels.WeldingRodRecord.Employee;
using Web.Helpers;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Approver")]
    public class EmployeeController : CrudBaseController<EmployeeModifyViewModel, EmployeeModifyViewModel, EmployeeDetailViewModel, EmployeeDetailViewModel, EmployeeSearchViewModel>
    {
        private readonly IEmployeeService<EmployeeModifyViewModel, EmployeeModifyViewModel, EmployeeDetailViewModel> _employeeService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeService<EmployeeModifyViewModel, EmployeeModifyViewModel, EmployeeDetailViewModel> employeeService, ILogger<EmployeeController> logger, IMapper mapper) : base(employeeService, logger, mapper, "Employee", "Accounts")
        {
            _employeeService = employeeService;
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
                    //new DataTableActionViewModel() {Action="Delete",Title="Delete",Href=$"/Employee/Delete/Id"},
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
                        Id = model.Id,
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
            var response = await _employeeService.ResetAccessCode(model);

            if (response.Status==System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("EmployeeId", "Employee Id already in use.");
            //if (Request.IsAjaxRequest())
            //{
            var errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                               .Select(m => m.ErrorMessage).ToList();

            return Json(new JsonResultViewModel
            {
                Success = false,
                Errors = errors
            });
            //}
           
        }
        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "First Name",data = "FirstName"},
                new DataTableViewModel{title = "Last Name",data = "LastName"},
                new DataTableViewModel{title = "Email",data = "Email"},
                //new DataTableViewModel{title = "Is Approver?",data = "IsApprover"},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-form-export"}

            };
        }
        public override async Task<ActionResult> Create(EmployeeModifyViewModel model)
        {
            bool isUnique = await _employeeService.IsEmployeeIdUnique(model.Id, model.EmployeeId);
            if (!isUnique)
            {
                ModelState.AddModelError("EmployeeId", "Employee Id already in use.");
            }
            return await base.Create(model);
        }
        public override async Task<ActionResult> Update(EmployeeModifyViewModel model)
        {
            bool isUnique = await _employeeService.IsEmployeeIdUnique(model.Id, model.EmployeeId);
            if (!isUnique)
            {
                ModelState.AddModelError("EmployeeId", "Employee Id already in use.");
            }
            return await base.Update(model);
        }
        public async Task<IActionResult> ValidateEmployeeId(int id, string employeeId)
        {
            return Json(await _employeeService.IsEmployeeIdUnique(id, employeeId));
        }
        public async Task<IActionResult> ValidateEmployeeEmail(int id, string email)
        {
            return Json(await _employeeService.IsEmployeeEmailUnique(id, email));
        }
    }
}
