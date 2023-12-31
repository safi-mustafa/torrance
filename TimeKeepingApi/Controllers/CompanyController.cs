﻿using System;
using Torrance.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Repositories.Services.CommonServices.CompanyService;
using ViewModels.Common.Company;
using Microsoft.AspNetCore.Authorization;
using DocumentFormat.OpenXml.Bibliography;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CompanyController : CrudBaseBriefController<CompanyDetailViewModel, CompanySearchViewModel>
    {
        public CompanyController(ICompanyService<CompanyCreateViewModel, CompanyModifyViewModel, CompanyDetailViewModel> CompanyService, ILogger<CompanyController> logger) : base(CompanyService, logger, "Company")
        {

        }
        public override Task<IActionResult> GetAll([FromQuery] CompanySearchViewModel search)
        {
            search.DisablePagination = true;
            return base.GetAll(search);
        }
    }
}

