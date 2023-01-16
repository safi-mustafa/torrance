using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pagination;
using Repositories.Services.CommonServices.ContractorService;
using Select2;
using ViewModels.Common.Contractor;
using ViewModels.CRUD;
using ViewModels.DataTable;

namespace Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ContractorController : CrudBaseController<ContractorModifyViewModel, ContractorModifyViewModel, ContractorDetailViewModel, ContractorDetailViewModel, ContractorSearchViewModel>
    {
        private readonly IContractorService _ContractorService;
        private readonly ILogger<ContractorController> _logger;

        public ContractorController(IContractorService ContractorService, ILogger<ContractorController> logger, IMapper mapper) : base(ContractorService, logger, mapper, "Contractor", "Contractors")
        {
            _ContractorService = ContractorService;
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
