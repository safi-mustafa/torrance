using System;
using TorranceApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Repositories.Services.CommonServices.ContractorService;
using ViewModels.Common.Contractor;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractorController : CrudBaseBriefController<ContractorModifyViewModel, ContractorModifyViewModel, ContractorDetailViewModel, ContractorDetailViewModel, ContractorSearchViewModel>
    {
        public ContractorController(IContractorService<ContractorCreateViewModel, ContractorModifyViewModel, ContractorDetailViewModel> contractorService) : base(contractorService)
        {
        }
    }
}

