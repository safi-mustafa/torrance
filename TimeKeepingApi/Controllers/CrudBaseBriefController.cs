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

namespace ChargieApi.Controllers
{

    [Controller]
    public abstract class CrudBaseBriefController<CreateViewModel, UpdateViewModel, DetailViewModel, PaginatedResultViewModel, SearchViewModel> : TorranceController
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where PaginatedResultViewModel : class, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
        where SearchViewModel : IBaseSearchModel, new()
    {
        private readonly IBaseCrud<CreateViewModel, UpdateViewModel, DetailViewModel> _service;

        public CrudBaseBriefController(IBaseCrud<CreateViewModel, UpdateViewModel, DetailViewModel> service)
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
