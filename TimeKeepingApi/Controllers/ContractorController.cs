using System;
using Torrance.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Repositories.Services.CommonServices.ContractorService;
using ViewModels.Common.Contractor;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ContractorController : CrudBaseBriefController<ContractorDetailViewModel, ContractorSearchViewModel>
    {
        public ContractorController(IContractorService<ContractorCreateViewModel, ContractorModifyViewModel, ContractorDetailViewModel> contractorService) : base(contractorService)
        {
        }
    }
}

