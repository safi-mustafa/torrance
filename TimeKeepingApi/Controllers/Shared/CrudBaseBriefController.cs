using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Common.Interfaces;
using Pagination;
using Repositories.Interfaces;
using System.Net;
using System.Security.Claims;
using ViewModels.Shared;

namespace Torrance.Api.Controllers
{

    [Controller]
    public abstract class CrudBaseBriefController<PaginatedResultViewModel, SearchViewModel> : TorranceController
        where PaginatedResultViewModel : class, new()
        where SearchViewModel : IBaseSearchModel, new()
    {
        private readonly IBaseSearch _service;

        public CrudBaseBriefController(IBaseSearch service)
        {
            this._service = service;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public virtual async Task<IActionResult> GetAll([FromQuery] SearchViewModel search)
        {
            var result = await _service.GetAll<PaginatedResultViewModel>(search);
            return ReturnProcessedResponse<PaginatedResultModel<PaginatedResultViewModel>>(result);
        }

    }
}
