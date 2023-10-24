using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Common.Interfaces;
using Newtonsoft.Json;
using Pagination;
using Repositories.Interfaces;
using System.Net;
using System.Security.Claims;
using ViewModels.Shared;

namespace Torrance.Api.Controllers
{

    [Controller]
    public abstract class CrudBaseController<CreateViewModel, UpdateViewModel, DetailViewModel, PaginatedResultViewModel, SearchViewModel> : TorranceController
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where PaginatedResultViewModel : class, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
        where SearchViewModel : IBaseSearchModel, new()
    {
        private readonly IBaseCrud<CreateViewModel, UpdateViewModel, DetailViewModel> _service;
        private readonly ILogger _logger;
        private readonly string _controllerName;

        public CrudBaseController(IBaseCrud<CreateViewModel, UpdateViewModel, DetailViewModel> service, ILogger logger, string controllerName) : base(logger, controllerName)
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
            _logger.LogInformation($"{_controllerName} -> GetAll: {JsonConvert.SerializeObject(search)}");
            var result = await _service.GetAll<PaginatedResultViewModel>(search);
            return ReturnProcessedResponse<PaginatedResultModel<PaginatedResultViewModel>>(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public virtual async Task<IActionResult> Get(long id)
        {
            var result = await _service.GetById(id);
            _logger.LogInformation($"{_controllerName} -> GetById({id}): {JsonConvert.SerializeObject(result)}");
            return ReturnProcessedResponse<DetailViewModel>(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public virtual async Task<IActionResult> Post([FromBody] CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation($"{_controllerName} -> Post: {JsonConvert.SerializeObject(model)}");
                var data = await _service.Create(model);
                return ReturnProcessedResponse(data);
            }
            //adding model state errors in the logs
            LogModelStateError(model, "POST");
            return ReturnProcessedResponse(new RepositoryResponse { Status = HttpStatusCode.BadRequest });
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public virtual async Task<IActionResult> Put([FromBody] UpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation($"{_controllerName} -> Put: {JsonConvert.SerializeObject(model)}");
                var result = await _service.Update(model);
                return ReturnProcessedResponse(result);
            }
            //adding model state errors in the logs
            LogModelStateError(model, "PUT");
            return ReturnProcessedResponse(new RepositoryResponse { Status = HttpStatusCode.BadRequest });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public virtual async Task<IActionResult> Delete(long id)
        {
            _logger.LogInformation($"{_controllerName} -> Delete: {id}");
            var property = await _service.Delete(id);
            return ReturnProcessedResponse(property);
        }
    }
}

