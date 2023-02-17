using AutoMapper;
using Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Services.AppSettingServices.CompanyManagerService;
using ViewModels.AppSettings.CompanyManager;
using ViewModels.DataTable;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator,Approver")]
    public class CompanyManagerController : UserController<CompanyManagerModifyViewModel, CompanyManagerModifyViewModel, CompanyManagerDetailViewModel, CompanyManagerDetailViewModel, CompanyManagerSearchViewModel>
    {
        private readonly string _controllerName = "CompanyManager";
        private readonly ICompanyManagerService<CompanyManagerModifyViewModel, CompanyManagerModifyViewModel, CompanyManagerDetailViewModel> _employeeService;
        private readonly ILogger<CompanyManagerController> _logger;

        public CompanyManagerController(ICompanyManagerService<CompanyManagerModifyViewModel, CompanyManagerModifyViewModel, CompanyManagerDetailViewModel> employeeService, ILogger<CompanyManagerController> logger, IMapper mapper) : base(employeeService, logger, mapper, "CompanyManager", "Company Managers", RolesCatalog.CompanyManager)
        {
            _employeeService = employeeService;
            _logger = logger;
        }
        protected override void SetDatatableActions<T>(DatatablePaginatedResultModel<T> result)
        {
            if (User.IsInRole("Approver") || User.IsInRole("SuperAdmin") || User.IsInRole("Administrator"))
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
                new DataTableViewModel{title = "Full Name",data = "FullName"},
                new DataTableViewModel{title = "Company",data = "Company.Name"},
                new DataTableViewModel{title = "Email",data = "Email"},
                new DataTableViewModel{title = "Access Code",data = "FormattedAccessCode"},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-form-export"}

            };
        }
    }
}
