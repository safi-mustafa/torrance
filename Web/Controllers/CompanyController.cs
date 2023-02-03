using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.CommonServices.CompanyService;
using ViewModels.Common.Company;
using ViewModels.DataTable;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class CompanyController : CrudBaseController<CompanyModifyViewModel, CompanyModifyViewModel, CompanyDetailViewModel, CompanyDetailViewModel, CompanySearchViewModel>
    {
        private readonly ICompanyService<CompanyModifyViewModel, CompanyModifyViewModel, CompanyDetailViewModel> _companyService;
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(ICompanyService<CompanyModifyViewModel, CompanyModifyViewModel, CompanyDetailViewModel> companyService, ILogger<CompanyController> logger, IMapper mapper) : base(companyService, logger, mapper, "Company", "Companies")
        {
            _companyService = companyService;
            _logger = logger;
        }

        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Name",data = "Name"},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-form-export"}

            };
        }
    }
}
