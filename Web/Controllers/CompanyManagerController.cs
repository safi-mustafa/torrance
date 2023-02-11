using AutoMapper;
using Enums;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.AppSettingServices.CompanyManagerService;
using ViewModels.AppSettings.CompanyManager;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Approver")]
    public class CompanyManagerController : UserController<CompanyManagerModifyViewModel, CompanyManagerModifyViewModel, CompanyManagerDetailViewModel, CompanyManagerDetailViewModel, CompanyManagerSearchViewModel>
    {
        private readonly ICompanyManagerService<CompanyManagerModifyViewModel, CompanyManagerModifyViewModel, CompanyManagerDetailViewModel> _employeeService;
        private readonly ILogger<CompanyManagerController> _logger;

        public CompanyManagerController(ICompanyManagerService<CompanyManagerModifyViewModel, CompanyManagerModifyViewModel, CompanyManagerDetailViewModel> employeeService, ILogger<CompanyManagerController> logger, IMapper mapper) : base(employeeService, logger, mapper, "CompanyManager", "Company Managers", RolesCatalog.CompanyManager)
        {
            _employeeService = employeeService;
            _logger = logger;
        }


    }
}
