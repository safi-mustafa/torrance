using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Common.Interfaces;
using Newtonsoft.Json;
using Pagination;
using Repositories.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
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
        private readonly ILogger _logger;
        private readonly string _controllerName;

        public CrudBaseBriefController(IBaseSearch service, ILogger logger, string controllerName)
        {
            _service = service;
            _logger = logger;
            _controllerName = controllerName;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public virtual async Task<IActionResult> GetAll([FromQuery] SearchViewModel search)
        {

            SetPagination(search);
            _logger.LogInformation($"{_controllerName} -> GetAll: {JsonConvert.SerializeObject(search)}");

            var result = await _service.GetAll<PaginatedResultViewModel>(search);
            return ReturnProcessedResponse<PaginatedResultModel<PaginatedResultViewModel>>(result);
        }

        protected virtual void SetPagination(SearchViewModel search)
        {
            search.DisablePagination = true;
        }

    }
}
