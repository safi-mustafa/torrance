using System;
using BainBridgeApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Repositories.Services.CommonServices.CompanyService;
using ViewModels.Common.Company;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CompanyController : CrudBaseBriefController<CompanyDetailViewModel, CompanySearchViewModel>
    {
        public CompanyController(ICompanyService<CompanyCreateViewModel, CompanyModifyViewModel, CompanyDetailViewModel> CompanyService) : base(CompanyService)
        {
        }
    }
}

