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
using Web.Controllers.SharedControllers;
using Newtonsoft.Json;
using ViewModels.OverrideLogs;
using Repositories.Services.CommonServices.ValidationService.UniqueNameService;
using ViewModels.Common.Validation;

namespace Web.Controllers
{
    public class JsonResultViewModel
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
    public abstract class CrudBaseController<CreateViewModel, UpdateViewModel, DetailViewModel, PaginatedResultViewModel, SearchViewModel> : DatatableBaseController<PaginatedResultViewModel, SearchViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where PaginatedResultViewModel : class, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
        where SearchViewModel : IBaseSearchModel, new()
    {
        private readonly IBaseCrud<CreateViewModel, UpdateViewModel, DetailViewModel> _service;
        private readonly ILogger<Controller> _logger;
        private readonly string _controllerName;
        private readonly string _title;
        private readonly bool _hideCreateButton;
        private readonly bool _useSameUpdateViews;
        private readonly IMapper _mapper;

        public CrudBaseController(IBaseCrud<CreateViewModel, UpdateViewModel, DetailViewModel> service, ILogger<Controller> logger, IMapper mapper, string controllerName, string title, bool hideCreateButton = false, bool useSameUpdateViews = true)
            :
            base(service, logger, mapper, controllerName, title, hideCreateButton, useSameUpdateViews)
        {
            _service = service;
            _logger = logger;
            _controllerName = controllerName;
            _title = title;
            _hideCreateButton = hideCreateButton;
            _useSameUpdateViews = useSameUpdateViews;
            _mapper = mapper;
        }

        #region[CRUD]

        public virtual ActionResult Create()
        {
            return UpdateView(GetUpdateViewModel("Create", null));
        }
        public virtual async Task<ActionResult> Update(int id)
        {
            try
            {
                var response = await _service.GetById(id);
                UpdateViewModel model = null;
                if (response.Status == System.Net.HttpStatusCode.OK)
                {
                    var parsedModel = response as RepositoryResponseWithModel<DetailViewModel>;
                    var responseModel = parsedModel?.ReturnModel;
                    model = _mapper.Map<UpdateViewModel>(responseModel);
                }
                if (model != null)
                {
                    return UpdateView(GetUpdateViewModel("Update", model));
                }
                else
                {
                    _logger.LogInformation($"{_title} with id " + id + "not found");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex) { _logger.LogError($"{_controllerName} GetById method threw an exception, Message: {ex.Message}"); return RedirectToAction("Index"); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Create(CreateViewModel model)
        {
            try
            {
                await ExecuteAdditionalValidation(model);
                if (ModelState.IsValid)
                {
                    var response = await _service.Create(model);
                    long id = 0;
                    if (response.Status == System.Net.HttpStatusCode.OK)
                    {
                        var parsedResponse = response as RepositoryResponseWithModel<long>;
                        id = parsedResponse?.ReturnModel ?? 0;
                    }
                    return PostModify(id, model, "Create");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{_controllerName} Create method threw an exception, Message: {ex.Message}");
            }
            return GetErrors("Create", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Update(UpdateViewModel model)
        {
            try
            {
                await ExecuteAdditionalValidation(model);
                if (ModelState.IsValid)
                {
                    long id = 0;
                    var response = await _service.Update(model);

                    if (response.Status == System.Net.HttpStatusCode.OK)
                    {
                        var parsedResponse = response as RepositoryResponseWithModel<long>;
                        id = parsedResponse?.ReturnModel ?? 0;
                    }
                    return PostModify(id, model, "Update");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{_controllerName} Update method threw an exception, Message: {ex.Message}");
            }

            return GetErrors("Update", model);
        }
        private ActionResult PostModify(long id, IBaseCrudViewModel model, string action)
        {
            if (id > 0)
            {
                var successMessage = $"{_controllerName}: Record with id: {id}" + action + "d" + "Successfully at " + DateTime.Now;
                _logger.LogInformation(successMessage);
                if (Request.IsAjaxRequest())
                {
                    return Json(new JsonResultViewModel { Success = true });
                }
                return RedirectToAction("Index");
            }
            else
            {
                return GetErrors(action, model, new List<string> { "Some error occured. Please try again later." });
            }
        }

        public virtual async Task<ActionResult> Detail(int id)
        {
            try
            {
                var response = await _service.GetById(id);
                if (response.Status == System.Net.HttpStatusCode.OK)
                {
                    var parsedResponse = response as RepositoryResponseWithModel<DetailViewModel>;
                    var model = parsedResponse?.ReturnModel ?? new();

                    return DetailView(SetDetailViewModel(model));
                }
                else
                {
                    _logger.LogInformation($"{_title} with id " + id + "not found");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex) { _logger.LogError($"{_controllerName} Detail method threw an exception, Message: {ex.Message}"); return RedirectToAction("Index"); }
        }

        public virtual async Task<ActionResult> Delete(int id)
        {
            try
            {
                var response = await _service.Delete(id);
                if (response.Status == System.Net.HttpStatusCode.OK)
                {
                    _logger.LogInformation($"{_controllerName}: Record with id: {id} Deleted  Successfully at " + DateTime.Now);
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

        //public virtual async Task<ActionResult> Approve(int id)
        //{
        //    try
        //    {
        //        if (await _service.Approve(id))
        //        {
        //            _logger.LogInformation($"{_controllerName}: Record with id: {id} Approved Successfully at " + DateTime.Now);
        //            return RedirectToAction("Index");
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex) { _logger.LogError($"{_controllerName} Approve method threw an exception for record with id: {id}, Message: {ex.Message}"); return RedirectToAction("Index"); }
        //}

        public virtual async Task<JsonResult> Select2(string prefix, int pageSize, int pageNumber, string customParams)
        {
            try
            {
                var svm = SetSelect2CustomParams(customParams);
                svm.PerPage = pageSize;
                svm.CalculateTotal = true;
                svm.CurrentPage = pageNumber;
                svm.Search = new DataTableSearchViewModel() { value = prefix };
                var response = await _service.GetAll<PaginatedResultViewModel>(svm);
                var totalCount = 0;
                PaginatedResultModel<PaginatedResultViewModel> items = new();
                if (response.Status == System.Net.HttpStatusCode.OK)
                {
                    var parsedResponse = response as RepositoryResponseWithModel<PaginatedResultModel<PaginatedResultViewModel>>;
                    items = parsedResponse?.ReturnModel ?? new();
                    totalCount = parsedResponse.ReturnModel._meta.TotalCount;
                }
                var select2List = GetSelect2List(items);
                return Json(new Select2Repository().GetSelect2PagedResult(pageSize, pageNumber, totalCount, select2List));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{_controllerName} Select2 method threw an exception, Message: {ex.Message}");
                return null;
            }
        }

        public virtual List<Select2OptionModel<ISelect2Data>> GetSelect2List(PaginatedResultModel<PaginatedResultViewModel> paginatedResult)
        {
            List<Select2OptionModel<ISelect2Data>> response = new List<Select2OptionModel<ISelect2Data>>();
            foreach (var item in paginatedResult.Items)
            {
                var select2Item = item as ISelect2Data;
                response.Add(new Select2OptionModel<ISelect2Data>
                {
                    id = select2Item.Id.ToString(),
                    text = select2Item.Name,
                    additionalAttributesModel = select2Item
                });
            }
            return response;
            //return response.OrderBy(m => m.id).ToList();
        }

        #endregion

        #region[Helper Models]

        protected ActionResult UpdateView(CrudUpdateViewModel vm)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Shared/Crud/UpdateView/_UpdateForm.cshtml", vm);
            }
            return View("~/Views/Shared/Crud/UpdateView/_UpdateForm.cshtml", vm);
        }

        protected ActionResult DetailView(CrudDetailViewModel vm)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Shared/Crud/DetailView/_DetailForm.cshtml", vm);
            }
            return View("~/Views/Shared/Crud/DetailView/_DetailForm.cshtml", vm);
        }

        protected virtual CrudUpdateViewModel SetUpdateViewModel(string title, IBaseCrudViewModel updateModel, string formAction = null, string formType = null, string updateViewPath = "", string formId = "")
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
            vm.UpdateModel = updateModel == null ? new CreateViewModel() : updateModel;
            return vm;

        }

        protected virtual CrudUpdateViewModel GetUpdateViewModel(string action, IBaseCrudViewModel model)
        {
            string updateViewPath = "";
            if (_useSameUpdateViews == false)
            {
                if (action == "Create")
                {
                    updateViewPath = "_Create";
                }
                else
                {
                    updateViewPath = "_Update";
                }
            }
            return SetUpdateViewModel($"{action} {_title}", model, action, null, updateViewPath);
        }

        protected virtual CrudDetailViewModel SetDetailViewModel(IBaseCrudViewModel model)
        {
            CrudDetailViewModel vm = new CrudDetailViewModel()
            {
                Title = $"{_title} Details",
                DetailModel = model
            };
            return vm;
        }

        protected virtual SearchViewModel SetSelect2CustomParams(string customParams)
        {
            if (customParams == null)
                return new SearchViewModel();
            var svm = JsonConvert.DeserializeObject<SearchViewModel>(customParams);
            return svm;
        }

        private ActionResult GetErrors(string action, IBaseCrudViewModel model, List<string> customErrors = null)
        {
            if (Request.IsAjaxRequest())
            {
                var errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                                   .Select(m => m.ErrorMessage).ToList();
                if (customErrors != null)
                    errors.AddRange(customErrors);
                return Json(new JsonResultViewModel
                {
                    Success = false,
                    Errors = errors

                });
            }
            return UpdateView(GetUpdateViewModel(action, model));
        }

        protected virtual async Task ExecuteAdditionalValidation(IBaseCrudViewModel model)
        {
            if (_service is IBaseServiceWithUniqueNameValidation && model is IValidateName)
            {
                var nameModel = (model as IValidateName);
                bool isvalid = await (_service as IBaseServiceWithUniqueNameValidation).IsUniqueName(nameModel);
                if (!isvalid)
                    ModelState.AddModelError("Name", $"The Name ({nameModel.Name}) already exist");
            }
        }
        #endregion

    }
}
