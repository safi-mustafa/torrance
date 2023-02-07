using Microsoft.AspNetCore.Mvc;
using Select2.Model;
using Select2;
using Repositories.Interfaces;
using Pagination;
using ViewModels.CRUD;
using ViewModels.DataTable;
using ViewModels.Shared;
using AutoMapper;
using Models.Common.Interfaces;
using Web.Helpers;
using Centangle.Common.ResponseHelpers.Models;
using Repositories.Shared.Interfaces;
using Enums;

namespace Web.Controllers
{

    public abstract class ApproveBaseController<Service, CreateViewModel, UpdateViewModel, DetailViewModel, PaginatedResultViewModel, SearchViewModel> : CrudBaseController<CreateViewModel, UpdateViewModel, DetailViewModel, PaginatedResultViewModel, SearchViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where PaginatedResultViewModel : class, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
        where SearchViewModel : IBaseSearchModel, new()
        where Service : IBaseCrud<CreateViewModel, UpdateViewModel, DetailViewModel>, IBaseApprove
    {
        private readonly Service _service;
        private readonly IBaseApprove _approveService;
        private readonly ILogger<Controller> _logger;
        private readonly string _controllerName;
        private readonly string _title;
        private readonly bool _hideCreateButton;
        private readonly bool _useSameUpdateViews;
        private readonly IMapper _mapper;
        public ApproveBaseController(Service service, ILogger<Controller> logger, IMapper mapper, string controllerName, string title, bool hideCreateButton = false, bool useSameUpdateViews = true) : base(service, logger, mapper, controllerName, title, hideCreateButton, useSameUpdateViews)
        {
            _service = service;
            _logger = logger;
            _controllerName = controllerName;
            _title = title;
            _hideCreateButton = hideCreateButton;
            _useSameUpdateViews = useSameUpdateViews;
            _mapper = mapper;
        }

        public async Task<ActionResult> ApproveRecords(List<long> Ids, bool Status)
        {
            try
            {
                await _service.ApproveRecords(Ids, Status);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Activity _ApproveTimesheets method threw an exception, Message: {ex.Message}");
                return null;
            }

        }
        public async Task<ActionResult> GetApprovedRecordIds()
        {
            try
            {
                var result = await _service.GetApprovedRecordIds();
                return Json(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Activity _CheckProjectCBOs method threw an exception, Message: {ex.Message}");
                return null;
            }

        }

        public async Task<bool> ApproveStatus(long id, Status status)
        {
            try
            {
                var response = await _service.SetApproveStatus(id, status);
                if (response.Status == System.Net.HttpStatusCode.OK)
                {
                    _logger.LogInformation($"{_controllerName}: Record with id: {id} Approved Successfully at " + DateTime.Now);
                    return true;
                }
                return false;
            }
            catch (Exception ex) { _logger.LogError($"{_controllerName} Approve method threw an exception for record with id: {id}, Message: {ex.Message}"); return false; }
        }
    }
}
