using Centangle.Common.ResponseHelpers.Models;
using Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Common.Interfaces;
using Newtonsoft.Json;
using Pagination;
using Repositories.Interfaces;
using Repositories.Shared.Interfaces;
using System.Net;
using ViewModels.Shared;

namespace Torrance.Api.Controllers
{

    [Controller]
    public abstract class ApproveCrudBaseController<Service, CreateViewModel, UpdateViewModel, DetailViewModel, PaginatedResultViewModel, SearchViewModel> : TorranceController
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where PaginatedResultViewModel : class, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
        where SearchViewModel : IBaseSearchModel, new()
        where Service : IBaseCrud<CreateViewModel, UpdateViewModel, DetailViewModel>, IBaseApprove
    {
        private readonly Service _service;
        private readonly ILogger _logger;
        private readonly string _controllerName;

        public ApproveCrudBaseController(Service service, ILogger logger, string controllerName)
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

        [Authorize(Roles = ("Admin,SuperAdmin,Approver"))]
        [HttpPut("{id}/{status}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public virtual async Task<IActionResult> Approve(long id, Status status)
        {
            _logger.LogInformation($"{_controllerName} -> Approve(id: {id}, status:{status})");
            var result = await _service.SetApproveStatus(id, status);
            return ReturnProcessedResponse(result);
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
                var data = await _service.Update(model);
                return ReturnProcessedResponse(data);
            }
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