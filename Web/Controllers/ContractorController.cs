using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.CommonServices.ContractorService;
using ViewModels.Common.Contractor;
using ViewModels.DataTable;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    public class ContractorController : CrudBaseController<ContractorModifyViewModel, ContractorModifyViewModel, ContractorDetailViewModel, ContractorDetailViewModel, ContractorSearchViewModel>
    {
        private readonly IContractorService<ContractorModifyViewModel, ContractorModifyViewModel, ContractorDetailViewModel> _ContractorService;
        private readonly ILogger<ContractorController> _logger;

        public ContractorController(IContractorService<ContractorModifyViewModel, ContractorModifyViewModel, ContractorDetailViewModel> ContractorService, ILogger<ContractorController> logger, IMapper mapper) : base(ContractorService, logger, mapper, "Contractor", "Contractors")
        {
            _ContractorService = ContractorService;
            _logger = logger;
        }

        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Name",data = "Name", orderable = true},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-from-export"}

            };
        }
    }
}
