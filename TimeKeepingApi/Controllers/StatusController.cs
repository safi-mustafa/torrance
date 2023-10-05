using System;
using Centangle.Common.ResponseHelpers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Shared.VersionService;
using Torrance.Api.Controllers;
using ViewModels.Authentication;

namespace API.Controllers
{
    public class StatusController : TorranceController
    {
        private readonly IVersionService _versionService;

        public StatusController(IVersionService versionService, ILogger<StatusController> logger) : base(logger, "Status")
        {
            this._versionService = versionService;
        }
        [HttpGet]
        [Route("/api/UpdateStatus")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateStatus()
        {
            var responseModel = new RepositoryResponseWithModel<UpdateStatusVM>();
            responseModel.ReturnModel = new UpdateStatusVM { LatestVersion = _versionService.GetLatestApiVersion(), IsForcible = _versionService.GetIsUpdateForcible() };
            return ReturnProcessedResponse<UpdateStatusVM>(responseModel);
        }
    }
}

