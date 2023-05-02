using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Common.Interfaces;
using Pagination;
using Repositories.Interfaces;
using Repositories.Shared.Interfaces;
using System.Net;
using System.Security.Claims;
using ViewModels.Shared;

namespace BainBridgeApi.Controllers
{

    [Controller]
    public abstract class ApproveCrudBaseController<Service, CreateViewModel, UpdateViewModel, DetailViewModel, PaginatedResultViewModel, SearchViewModel> : BainBridgeController
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where PaginatedResultViewModel : class, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
        where SearchViewModel : IBaseSearchModel, new()
        where Service : IBaseCrud<CreateViewModel, UpdateViewModel, DetailViewModel>, IBaseApprove
    {
        private readonly Service _service;

        public ApproveCrudBaseController(Service service)
        {
            _service = service;
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

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public virtual async Task<IActionResult> Get(long id)
        {
            var result = await _service.GetById(id);
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
            var property = await _service.Delete(id);
            return ReturnProcessedResponse(property);
        }
    }
}