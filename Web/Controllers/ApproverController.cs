using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pagination;
using Repositories.Services.WeldRodRecordServices.ApproverService;
using Repositories.Services.WeldRodRecordServices.EmployeeService;
using Select2.Model;
using ViewModels.Authentication;
using ViewModels.Authentication.Approver;
using ViewModels.DataTable;
using ViewModels.WeldingRodRecord.Employee;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class ApproverController : CrudBaseController<ApproverModifyViewModel, ApproverModifyViewModel, ApproverDetailViewModel, ApproverDetailViewModel, ApproverSearchViewModel>
    {
        private readonly IApproverService<ApproverModifyViewModel, ApproverModifyViewModel, ApproverDetailViewModel> _approverService;
        private readonly IEmployeeService<EmployeeModifyViewModel, EmployeeModifyViewModel, EmployeeDetailViewModel> _employeeService;
        private readonly ILogger<ApproverController> _logger;

        public ApproverController(IApproverService<ApproverModifyViewModel, ApproverModifyViewModel, ApproverDetailViewModel> approverService, IEmployeeService<EmployeeModifyViewModel, EmployeeModifyViewModel, EmployeeDetailViewModel> employeeService, ILogger<ApproverController> logger, IMapper mapper) : base(approverService, logger, mapper, "Approver", "Approvers")
        {
            _approverService = approverService;
            _employeeService = employeeService;
            _logger = logger;
        }

        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Email",data = "Email"},
                new DataTableViewModel{title = "Phone #",data = "PhoneNumber"},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-form-export"}

            };
        }
        protected override void SetDatatableActions<T>(DatatablePaginatedResultModel<T> result)
        {
            //if (User.IsInRole("Approver") || User.IsInRole("SuperAdmin"))
            //{
                result.ActionsList = new List<DataTableActionViewModel>()
                {
                    new DataTableActionViewModel() {Action="ResetPassword",Title="ResetPassword",Href=$"/Approver/ResetPassword/Id"},
                    new DataTableActionViewModel() {Action="Detail",Title="Detail",Href=$"/Approver/Detail/Id"},
                    new DataTableActionViewModel() {Action="Update",Title="Update",Href=$"/Approver/Update/Id"},
                    new DataTableActionViewModel() {Action="Delete",Title="Delete",Href=$"/Approver/Delete/Id"},
                };
         //   }
         

        }


        public override async Task<ActionResult> Create(ApproverModifyViewModel model)
        {
            bool isUnique = await _approverService.IsAccessCodeUnique(model.Id, model.AccessCode);
            if (!isUnique)
            {
                ModelState.AddModelError("AccessCode", "Access Code already in use.");
            }
            return await base.Create(model);
        }
        public override async Task<ActionResult> Update(ApproverModifyViewModel model)
        {
            bool isUnique = await _approverService.IsAccessCodeUnique(model.Id, model.AccessCode);
            if (!isUnique)
            {
                ModelState.AddModelError("AccessCode", "Access Code already in use.");
            }
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            ModelState.Remove("AccessCode");
            ModelState.Remove("ConfirmAccessCode");
            return await base.Update(model);
        }
        public async Task<IActionResult> ValidateApproverEmail(long id, string email)
        {
            return Json(await _approverService.IsApproverEmailUnique(id, email));
        }
        public async Task<IActionResult> ValidateAccessCode(long id, string accessCode)
        {
            return Json(await _approverService.IsAccessCodeUnique(id, accessCode));
        }

        public async Task<ActionResult> ResetPassword(int id)
        {
            try
            {
                var response = await _approverService.GetById(id);
                if (response.Status == System.Net.HttpStatusCode.OK)
                {
                    var parsedResponse = response as RepositoryResponseWithModel<ApproverDetailViewModel>;
                    var model = parsedResponse?.ReturnModel ?? new();
                    ChangeAccessCodeVM viewModel = new ChangeAccessCodeVM
                    {
                        Id = model.Id,
                        UserId = model.Id,
                        CurrentAccessCode = model.AccessCode
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
            return RedirectToAction("Index");
        }

        protected override ApproverSearchViewModel SetSelect2CustomParams(string customParams)
        {
            var svm = JsonConvert.DeserializeObject<ApproverSearchViewModel>(customParams);
            return svm;
        }

        public override List<Select2OptionModel<ISelect2Data>> GetSelect2List(PaginatedResultModel<ApproverDetailViewModel> paginatedResult)
        {
            List<Select2OptionModel<ISelect2Data>> response = new List<Select2OptionModel<ISelect2Data>>();
            foreach (var item in paginatedResult.Items)
            {
                response.Add(new Select2OptionModel<ISelect2Data>
                {
                    id = item.Id.ToString(),
                    text = item.Email
                });
            }

            return response.OrderBy(m => m.id).ToList();
        }
    }
}
