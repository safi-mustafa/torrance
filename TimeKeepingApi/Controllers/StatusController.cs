using System;
using Centangle.Common.ResponseHelpers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Shared.VersionService;
using Torrance.Api.Controllers;
using ViewModels.Authentication;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateStatusController : TorranceController
    {
        private readonly IVersionService _versionService;

        public UpdateStatusController(IVersionService versionService)
        {
            this._versionService = versionService;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var responseModel = new RepositoryResponseWithModel<UpdateStatusVM>();
            responseModel.ReturnModel = new UpdateStatusVM { LatestVersion = _versionService.GetLatestApiVersion(), IsForcible = _versionService.GetIsUpdateForcible() };
            return ReturnProcessedResponse<UpdateStatusVM>(responseModel);
        }
    }
}

