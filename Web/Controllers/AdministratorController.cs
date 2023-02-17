using AutoMapper;
using Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Services.AppSettingServices.AdministratorService;
using ViewModels.AppSettings.Administrator;
using ViewModels.DataTable;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator,Approver")]
    public class AdministratorController : UserController<AdministratorModifyViewModel, AdministratorModifyViewModel, AdministratorDetailViewModel, AdministratorDetailViewModel, AdministratorSearchViewModel>
    {
        private readonly string _controllerName = "Administrator";
        private readonly IAdministratorService<AdministratorModifyViewModel, AdministratorModifyViewModel, AdministratorDetailViewModel> _employeeService;
        private readonly ILogger<AdministratorController> _logger;

        public AdministratorController(IAdministratorService<AdministratorModifyViewModel, AdministratorModifyViewModel, AdministratorDetailViewModel> employeeService, ILogger<AdministratorController> logger, IMapper mapper) : base(employeeService, logger, mapper, "Administrator", "Administrators", RolesCatalog.Administrator)
        {
            _employeeService = employeeService;
            _logger = logger;
        }
        protected override void SetDatatableActions<T>(DatatablePaginatedResultModel<T> result)
        {
            //if (User.IsInRole("Approver") || User.IsInRole("SuperAdmin"))
            //{
            //    result.ActionsList = new List<DataTableActionViewModel>()
            //    {
            //        new DataTableActionViewModel() {Action="ResetPassword",Title="ResetPassword",Href=$"/{_controllerName}/ResetPassword/Id"},
            //    };
            //}
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
                new DataTableViewModel{title = "Full Name",data = "FullName"},
                //new DataTableViewModel{title = "Company",data = "Company.Name"},
                new DataTableViewModel{title = "Email",data = "Email"},
                //new DataTableViewModel{title = "Access Code",data = "FormattedAccessCode"},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-form-export"}

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

        public override async Task<bool> IsAccessCodeUnique(AdministratorModifyViewModel model)
        {
            
            return await Task.FromResult(true);
        }
    }
}
