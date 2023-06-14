using System;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using ViewModels.Authentication.Approver;
using Web.Controllers.SharedControllers;
using AutoMapper;
using Repositories.Services.AppSettingServices.ApproverService;
using ViewModels.DataTable;
using Repositories.Services.CommonServices.ApprovalService.Interface;
using ViewModels.Common;
using Microsoft.AspNetCore.Mvc;
using Enums;
using Centangle.Common.ResponseHelpers.Models;
using ViewModels.TimeOnTools.TOTLog;
using Repositories.Services.TimeOnToolServices.TOTLogService;
using Repositories.Services.AppSettingServices.WRRLogService;
using ViewModels.WeldingRodRecord.WRRLog;
using Helpers.Extensions;
using ViewModels.CRUD;
using ViewModels.Shared;
using Web.Helpers;
using ViewModels.OverrideLogs.ORLog;
using Repositories.Services.OverrideLogServices.ORLogService;
using static Repositories.Services.CommonServices.ApprovalService.ApprovalService;
using ViewModels.Authentication.User;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator,Approver")]
    public class ApprovalController : DatatableBaseController<ApprovalDetailViewModel, ApprovalSearchViewModel>
    {
        private readonly IApprovalService _approvalService;
        private readonly ITOTLogService<TOTLogModifyViewModel, TOTLogModifyViewModel, TOTLogDetailViewModel> _totService;
        private readonly IWRRLogService<WRRLogModifyViewModel, WRRLogModifyViewModel, WRRLogDetailViewModel> _wrrService;
        private readonly IORLogService<ORLogModifyViewModel, ORLogModifyViewModel, ORLogDetailViewModel> _overrideLogService;
        private readonly ILogger<ApprovalController> _logger;
        private readonly IMapper _mapper;
        private string _controllerName;
        private string _updateViewPath;
        private string _detailViewPath;
        private string _detailTitle;

        public
            ApprovalController(
            IApprovalService approvaleService,
            ITOTLogService<TOTLogModifyViewModel, TOTLogModifyViewModel, TOTLogDetailViewModel> totService,
            IWRRLogService<WRRLogModifyViewModel, WRRLogModifyViewModel, WRRLogDetailViewModel> wrrService,
            IORLogService<ORLogModifyViewModel, ORLogModifyViewModel, ORLogDetailViewModel> overrideLogService,
            ILogger<ApprovalController> logger,
            IMapper mapper
            ) : base(approvaleService, logger, mapper, "Approval", "Approvals", true)
        {
            _approvalService = approvaleService;
            _totService = totService;
            _wrrService = wrrService;
            _overrideLogService = overrideLogService;
            _logger = logger;
            _mapper = mapper;
        }
        protected override ApprovalSearchViewModel SetDefaultFilters()
        {
            var filters = base.SetDefaultFilters();
            filters.Status = Status.Pending;
            return filters;
        }

        public async Task<IActionResult> Detail(long id, LogType type)
        {
            try
            {
                return await SetDetailView(id, type, "~/Views/Shared/Crud/DetailView/_DetailForm.cshtml", false);
            }
            catch (Exception ex) { _logger.LogError($"{_controllerName} Detail method threw an exception, Message: {ex.Message}"); }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(long id, LogType type)
        {
            try
            {
                IRepositoryResponse response = new RepositoryResponse();
                if (type == LogType.TimeOnTools)
                    response = await _totService.Delete(id);
                else if (type == LogType.WeldingRodRecord)
                    response = await _wrrService.Delete(id);
                else if (type == LogType.Override)
                    response = await _overrideLogService.Delete(id);
                else
                    throw new Exception("Log type not found");

                if (response.Status == System.Net.HttpStatusCode.OK)
                {
                    _logger.LogInformation($"Approval for LogType: {type.GetDisplayName()} Record with id: {id} Deleted  Successfully at " + DateTime.UtcNow);
                    return Json(new
                    {
                        Success = true,
                        ReloadDatatable = true,
                    });
                }
                return Json(false);
            }
            catch (Exception ex) { _logger.LogError($"{_controllerName} Delete method threw an exception for record with id: {id}, Message: {ex.Message}"); return Json(false); }
        }

        public async Task<IActionResult> Update(long id, LogType type)
        {
            return await SetUpdateView(id, type, "Update");
        }

        public async Task<IActionResult> Approve(long id, LogType type)
        {
            return await SetUpdateView(id, type, "Approve");
        }

        [AllowAnonymous]
        public async Task<IActionResult> ApproveByNotification(Guid id, string message = "")
        {
            var response = await _approvalService.GetLogIdAndTypeFromNotificationId(id);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                var parsedModel = response as RepositoryResponseWithModel<LogDetailFromNotification>;
                var responseModel = parsedModel?.ReturnModel;
                try
                {
                    ViewBag.Message = message;
                    return await SetDetailView(responseModel.LogId, responseModel.LogType, "~/Views/Approval/DetailForUnAuthenticatedApprove.cshtml", true, id, responseModel.ApproverId);
                }
                catch (Exception ex)
                {

                }
            }
            return RedirectToAction("ShowMessage", "Home", new { message = "No such Log exists" });
        }

        private async Task<IActionResult> SetUpdateView(long id, LogType type, string view)
        {
            try
            {
                IRepositoryResponse response;
                if (type == LogType.TimeOnTools)
                {
                    _controllerName = "TOTLog";
                    _updateViewPath = "~/Views/TOTLog/_Update.cshtml";
                    response = await _totService.GetById(id);

                    return GetUpdateView<TOTLogModifyViewModel, TOTLogDetailViewModel>(response, id, type, view);
                }
                else if (type == LogType.WeldingRodRecord)
                {
                    _controllerName = "WRRLog";
                    _updateViewPath = "~/Views/WRRLog/_Update.cshtml";
                    response = await _wrrService.GetById(id);

                    return GetUpdateView<WRRLogModifyViewModel, WRRLogDetailViewModel>(response, id, type, view);
                }
                else
                {
                    _controllerName = "OverrideLog";
                    _updateViewPath = "~/Views/OverrideLog/_Update.cshtml";
                    response = await _overrideLogService.GetById(id);

                    return GetUpdateView<ORLogModifyViewModel, ORLogDetailViewModel>(response, id, type, view);
                }
            }
            catch (Exception ex) { _logger.LogError($"Approval for LogType: {type.GetDisplayName()} GetById method threw an exception, Message: {ex.Message}"); }
            return RedirectToAction("Index");
        }
        #region Helping Methods

        private ActionResult GetUpdateView<UpdateViewModel, DetailViewModel>(IRepositoryResponse response, long id, LogType type, string view)
            where UpdateViewModel : BaseCrudViewModel, new()
            where DetailViewModel : BaseCrudViewModel, new()
        {
            var model = new BaseCrudViewModel();
            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                var parsedModel = response as RepositoryResponseWithModel<DetailViewModel>;
                var responseModel = parsedModel?.ReturnModel;
                model = _mapper.Map<UpdateViewModel>(responseModel);
            }
            if (model != null)
            {
                return UpdateView(GetUpdateViewModel("Update", model, view));
            }
            else
            {
                _logger.LogInformation($"Approval for LogType: {type.GetDisplayName()} with id " + id + "not found");
                return RedirectToAction("Index");
            }
        }

        protected virtual CrudUpdateViewModel GetUpdateViewModel(string action, BaseCrudViewModel model, string view)
        {
            return SetUpdateViewModel($"{action} {_controllerName}", model, action, null, _updateViewPath, "", view);
        }

        protected virtual CrudUpdateViewModel SetUpdateViewModel(string title, BaseCrudViewModel updateModel, string formAction = null, string formType = null, string updateViewPath = "", string formId = "", string view = "")
        {
            CrudUpdateViewModel vm = new()
            {
                Title = title
            };
            if (!string.IsNullOrEmpty(formType))
            {
                vm.FormType = formType;
            }
            if (!string.IsNullOrEmpty(formAction))
            {
                vm.FormAction = formAction;
                vm.FormController = _controllerName;
            }
            if (!string.IsNullOrEmpty(updateViewPath))
            {
                vm.UpdateViewPath = updateViewPath;
            }
            if (!string.IsNullOrEmpty(formId))
            {
                vm.FormId = formId;
            }
            vm.IsApprovalForm = view == "Approve" ? true : false;
            vm.UpdateModel = updateModel == null ? new() : updateModel;
            return vm;
        }

        protected ActionResult UpdateView(CrudUpdateViewModel vm)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Shared/Crud/UpdateView/_UpdateForm.cshtml", vm);
            }
            return View("~/Views/Shared/Crud/UpdateView/_UpdateForm.cshtml", vm);
        }

        protected override void SetDatatableActions<T>(DatatablePaginatedResultModel<T> result)
        {
            result.ActionsList = new List<DataTableActionViewModel>()
            {
                    new DataTableActionViewModel() {Action="Detail",Title="Detail",Href=$"/Approval/Detail?Id&Type"},
                    new DataTableActionViewModel() {Action="Update",Title="Update",Href=$"/Approval/Update?Id&Type"},
                    new DataTableActionViewModel() {Action="Delete",Title="Delete",Href=$"/Approval/Delete?Id&Type"},
                    //new DataTableActionViewModel() {Action="Approve",Title="Approve",Href=$"/Approval/Approve?Id&Type"},
            };
        }

        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Status",data = "FormattedStatus",format="html",formatValue="status",exportColumn="FormattedStatus"},
                new DataTableViewModel{title = "Date",data = "FormattedDate",sortingColumn = "Date", orderable=true},
                new DataTableViewModel{title = "Type",data = "FormattedLogType", sortingColumn = "Type", orderable=true},
                new DataTableViewModel{title = "Requester",data = "Requester",sortingColumn="Employee.FullName", orderable=true},
                //new DataTableViewModel{title = "Approver",data = "Approver"},
                new DataTableViewModel{title = "Department",data = "Department",orderable=true},
                //new DataTableViewModel{title = "Contractor",data = "Contractor"},
                new DataTableViewModel{title = "Unit",data = "Unit", orderable=true},
                //new DataTableViewModel{title = "TWR",data = "TWR"},
                //new DataTableViewModel{title = "Reason",data = "Reason", orderable=true},
                new DataTableViewModel{title = "Total Hours",data = "TotalHours", orderable=true},
                new DataTableViewModel{title = "Total Head Count",data = "TotalHeadCount", orderable=true},
                new DataTableViewModel{title = "Total Cost",data = "TotalCost", orderable=true, className="dt-currency"},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-from-export"}

            };
        }

        protected override CrudListViewModel OverrideCrudListVM(CrudListViewModel vm)
        {
            vm.DataTableHeaderHtml = @"
                    <div class=""p-2 row"">
                        <span class=""badge Submitted m-1""> </span>
                        <span class=""stat-name"">Pending</span>
                    </div>";
            vm.IsResponsiveDatatable = false;
            return vm;
        }

        protected virtual CrudDetailViewModel SetDetailViewModel(IBaseCrudViewModel model, bool isApproval)
        {
            CrudDetailViewModel vm = new CrudDetailViewModel()
            {
                Title = $"{_detailTitle} Details",
                DetailViewPath = _detailViewPath,
                DetailModel = model,
                IsApprovalForm = isApproval
            };
            return vm;
        }

        private ActionResult GetDetailView<DetailViewModel>(IRepositoryResponse response, long id, LogType type, bool isApproval, string view) where DetailViewModel : BaseCrudViewModel, new()
        {
            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                var parsedResponse = response as RepositoryResponseWithModel<DetailViewModel>;
                var model = parsedResponse?.ReturnModel ?? new();
                var vm = SetDetailViewModel(model, isApproval);
                return View(view, vm);
            }
            else
            {
                _logger.LogInformation($"Approval for Detail of LogType: {type.GetDisplayName()} with id " + id + "not found");
                return RedirectToAction("Index");
            }
        }

        private async Task<IActionResult> SetDetailView(long id, LogType type, string view, bool isUnauthenticatedApproval, Guid notificationId = new Guid(), long approverId = 0)
        {
            IRepositoryResponse response = new RepositoryResponse();
            var isApproval = true;
            ViewBag.IgnoreLayout = true;
            if (type == LogType.TimeOnTools)
            {
                _detailViewPath = "~/Views/TOTLog/_Detail.cshtml";
                response = await _totService.GetById(id);
                var parsedModel = response as RepositoryResponseWithModel<TOTLogDetailViewModel>;
                isApproval = SetApproverValues(isUnauthenticatedApproval, notificationId, approverId, parsedModel);
                return GetDetailView<TOTLogDetailViewModel>(parsedModel, id, type, isApproval, view);
            }
            else if (type == LogType.WeldingRodRecord)
            {
                _detailViewPath = "~/Views/WRRLog/_Detail.cshtml";
                response = await _wrrService.GetById(id);
                var parsedModel = response as RepositoryResponseWithModel<WRRLogDetailViewModel>;
                isApproval = SetApproverValues(isUnauthenticatedApproval, notificationId, approverId, parsedModel);
                return GetDetailView<WRRLogDetailViewModel>(parsedModel, id, type, isApproval, view);
            }
            else
            {
                _detailViewPath = "~/Views/OverrideLog/_Detail.cshtml";
                response = await _overrideLogService.GetById(id);
                var parsedModel = response as RepositoryResponseWithModel<ORLogDetailViewModel>;
                isApproval = SetApproverValues(isUnauthenticatedApproval, notificationId, approverId, parsedModel);
                return GetDetailView<ORLogDetailViewModel>(parsedModel, id, type, isApproval, view);
            }
        }

        private static bool SetApproverValues<T>(bool isUnauthenticatedApproval, Guid notificationId, long approverId, RepositoryResponseWithModel<T>? parsedModel)
            where T : LogCommonDetailViewModel, new()
        {
            bool isApproval = false;
            if (!isUnauthenticatedApproval)
                return true;
            if (approverId > 0 && isUnauthenticatedApproval && parsedModel != null)
            {
                parsedModel.ReturnModel.Approver = new ApproverBriefViewModel { Id = approverId };
                parsedModel.ReturnModel.IsUnauthenticatedApproval = isUnauthenticatedApproval;
                parsedModel.ReturnModel.NotificationId = notificationId;
                isApproval = parsedModel.ReturnModel.Status == Status.Pending;
            }
            return isApproval;
        }


        #endregion
    }
}

