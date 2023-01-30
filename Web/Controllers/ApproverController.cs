using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Services.WeldRodRecordServices.ApproverService;
using ViewModels.Authentication.Approver;
using ViewModels.DataTable;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class ApproverController : CrudBaseController<UserUpdateViewModel, UserUpdateViewModel, UserDetailViewModel, UserDetailViewModel, UserSearchViewModel>
    {
        private readonly IApproverService<UserUpdateViewModel, UserUpdateViewModel, UserDetailViewModel> _approverService;
        private readonly ILogger<ApproverController> _logger;

        public ApproverController(IApproverService<UserUpdateViewModel, UserUpdateViewModel, UserDetailViewModel> employeeService, ILogger<ApproverController> logger, IMapper mapper) : base(employeeService, logger, mapper, "Approver", "Approvers")
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
        public override async Task<ActionResult> Update(UserUpdateViewModel model)
        {
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            return await base.Update(model);
        }
        public async Task<IActionResult> ValidateApproverEmail(int id, string email)
        {
            return Json(await _approverService.IsApproverEmailUnique(id, email));
        }
    }
}
