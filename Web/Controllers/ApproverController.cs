using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pagination;
using Repositories.Services.WeldRodRecordServices.ApproverService;
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
        private readonly ILogger<ApproverController> _logger;

        public ApproverController(IApproverService<ApproverModifyViewModel, ApproverModifyViewModel, ApproverDetailViewModel> employeeService, ILogger<ApproverController> logger, IMapper mapper) : base(employeeService, logger, mapper, "Approver", "Approvers")
        {
            _approverService = employeeService;
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
