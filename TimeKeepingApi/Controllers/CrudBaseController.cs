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
    public abstract class CrudBaseController<CreateViewModel, UpdateViewModel, DetailViewModel, PaginatedResultViewModel, SearchViewModel> : TorranceController
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where PaginatedResultViewModel : class, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
        where SearchViewModel : IBaseSearchModel, new()
    {
        private readonly IBaseCrud<CreateViewModel, UpdateViewModel, DetailViewModel> _service;

        public CrudBaseController(IBaseCrud<CreateViewModel, UpdateViewModel, DetailViewModel> service)
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

        //[HttpPost]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
        //[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        //public virtual async Task<IActionResult> Post([FromBody] UserRoleCreateVM<TRoleKey, TUserKey, TClientKey> model)
        //{
        //    var data = await _service.Create(model);
        //    await _uow.Commit();
        //    return ReturnProcessedResponse(data);
        //}

        //[HttpPut]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
        //[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        //public virtual async Task<IActionResult> Put([FromBody] UserRoleUpdateVM<TRoleKey, TUserKey> model)
        //{
        //    var result = await _service.Update(model);
        //    await _uow.Commit();
        //    return ReturnProcessedResponse(result);
        //}

        //[HttpDelete("{id}")]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
        //[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        //public virtual async Task<IActionResult> Delete(TRoleKey id)
        //{
        //    var property = await _service.Delete(id);
        //    await _uow.Commit();
        //    return ReturnProcessedResponse(property);
        //}
    }
}
