using AutoMapper;
using Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repositories.Services.AppSettingServices.CompanyManagerService;
using ViewModels.AppSettings.CompanyManager;
using ViewModels.DataTable;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator,Approver")]
    public class TimeKeeperController : UserController<CompanyManagerModifyViewModel, CompanyManagerModifyViewModel, CompanyManagerDetailViewModel, CompanyManagerDetailViewModel, CompanyManagerSearchViewModel>
    {
        private readonly string _controllerName = "TimeKeeper";
        private readonly ICompanyManagerService<CompanyManagerModifyViewModel, CompanyManagerModifyViewModel, CompanyManagerDetailViewModel> _employeeService;
        private readonly ILogger<TimeKeeperController> _logger;
        private readonly UserManager<ToranceUser> _userManager;

        public TimeKeeperController(ICompanyManagerService<CompanyManagerModifyViewModel, CompanyManagerModifyViewModel, CompanyManagerDetailViewModel> employeeService, ILogger<TimeKeeperController> logger, IMapper mapper, UserManager<ToranceUser> userManager) : base(employeeService, logger, mapper,userManager, "TimeKeeper", "Time Keeper", RolesCatalog.CompanyManager)
        {
            _employeeService = employeeService;
            _logger = logger;
            _userManager = userManager;
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
                new DataTableViewModel{title = "Full Name",data = "FullName",orderable=true},
                new DataTableViewModel{title = "Company",data = "Company.Name", orderable = true},
                new DataTableViewModel{title = "Email",data = "Email", orderable = true},
                new DataTableViewModel{title = "Access Code",data = "FormattedAccessCode", orderable = true},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-form-export"}

            };
        }
    }
}
