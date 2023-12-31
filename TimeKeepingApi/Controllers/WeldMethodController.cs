﻿using System;
using Torrance.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Repositories.Services.AppSettingServices.WeldMethodService;
using ViewModels.WeldingRodRecord.WeldMethod;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WeldMethodController : CrudBaseBriefController<WeldMethodDetailViewModel, WeldMethodSearchViewModel>
    {
        public WeldMethodController(IWeldMethodService<WeldMethodModifyViewModel, WeldMethodModifyViewModel, WeldMethodDetailViewModel> weldMethodService, ILogger<WeldMethodController> logger) : base(weldMethodService, logger, "Weld Method")
        {
        }
    }
}

