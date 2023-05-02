using AutoMapper;
using Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repositories.Services.AppSettingServices.AdministratorService;
using ViewModels.AppSettings.Administrator;
using ViewModels.Authentication.User;
using ViewModels.DataTable;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator,Approver")]
    public class AdministratorController : UserController<AdministratorModifyViewModel, AdministratorModifyViewModel, AdministratorDetailViewModel, AdministratorDetailViewModel, AdministratorSearchViewModel>
    {
        private readonly string _controllerName = "Administrator";
        private readonly IAdministratorService<AdministratorModifyViewModel, AdministratorModifyViewModel, AdministratorDetailViewModel> _adminService;
        private readonly ILogger<AdministratorController> _logger;
        private readonly UserManager<ToranceUser> _userManager;

        public AdministratorController(IAdministratorService<AdministratorModifyViewModel, AdministratorModifyViewModel, AdministratorDetailViewModel> adminService, ILogger<AdministratorController> logger, IMapper mapper,UserManager<ToranceUser> userManager) : base(adminService, logger, mapper, userManager, "Administrator", "Master Admin", RolesCatalog.Administrator)
        {
            _adminService = adminService;
            _logger = logger;
            _userManager = userManager;
        }
        protected override void SetDatatableActions<T>(DatatablePaginatedResultModel<T> result)
        {
            if (User.IsInRole("SuperAdmin"))
            {
                result.ActionsList = new List<DataTableActionViewModel>()
                {
                    new DataTableActionViewModel() {Action="ResetPassword",Title="ResetPassword",Href=$"/{_controllerName}/ResetPassword/Id"},
                };
            }
            if (User.IsInRole("SuperAdmin") || User.IsInRole("Administrator"))
            {
                result.ActionsList.AddRange(new List<DataTableActionViewModel>()
                {
                    new DataTableActionViewModel() {Action="Detail",Title="Detail",Href=$"/{_controllerName}/Detail/Id"},
                    new DataTableActionViewModel() {Action="Update",Title="Update",Href=$"/{_controllerName}/Update/Id"},
                    new DataTableActionViewModel() {Action="Delete",Title="Delete",Href=$"/{_controllerName}/Delete/Id"},
                });
            }

        }
        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Full Name",data = "FullName",orderable=true},
                //new DataTableViewModel{title = "Company",data = "Company.Name"},
                new DataTableViewModel{title = "Email",data = "Email",orderable=true},
                //new DataTableViewModel{title = "Access Code",data = "FormattedAccessCode"},
                new DataTableViewModel{title = "Status",data = "FormattedStatus", orderable = false},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-from-export"}

            };
        }

        public override Task<ActionResult> Create(AdministratorModifyViewModel model)
        {
            model.AccessCode = Guid.NewGuid().ToString();
            ModelState.Remove("Company.Id");
            ModelState.Remove("Company.Name");
            ModelState.Remove("AccessCode");
            return base.Create(model);
        }
        public override Task<ActionResult> Update(AdministratorModifyViewModel model)
        {
            ModelState.Remove("Company.Id");
            ModelState.Remove("Company.Name");
            ModelState.Remove("AccessCode");
            return base.Update(model);
        }
        public override async Task<bool> IsAccessCodeUnique(UserUpdateViewModel model)
        {
            return await Task.FromResult(true);
        }
        public async Task<bool> ValidatePassword(string password)
        {
            var response = await _adminService.ValidatePassword(password);
            return response;
        }
    }
}
