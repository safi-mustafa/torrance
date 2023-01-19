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

namespace Web.Controllers
{
    public class JsonResultViewModel
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
    public abstract class CrudBaseController<CreateViewModel, UpdateViewModel, DetailViewModel, PaginatedResultViewModel, SearchViewModel> : Controller
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
        public CrudBaseController()
        {

        }
        public CrudBaseController(IBaseCrud<CreateViewModel, UpdateViewModel, DetailViewModel> service, ILogger<Controller> logger, IMapper mapper, string controllerName, string title, bool hideCreateButton = false, bool useSameUpdateViews = true)
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
        public virtual ActionResult Index()
        {
            var vm = new CrudListViewModel();
            vm.Title = _title;
            vm.Filters = new SearchViewModel();
            vm.DatatableColumns = GetColumns();
            vm.DisableSearch = false;
            vm.HideCreateButton = _hideCreateButton;
            vm.ControllerName = _controllerName;
            vm.DataUrl = $"/{_controllerName}/Search";
            vm.SearchViewPath = $"~/Views/{_controllerName}/_Search.cshtml";
            return DataTableIndexView(vm);
        }

        public virtual async Task<JsonResult> Search(SearchViewModel searchModel)
        {
            try
            {
                var response = await _service.GetAll<PaginatedResultViewModel>(searchModel);
                PaginatedResultModel<PaginatedResultViewModel> model = new();
                if (response.Status == System.Net.HttpStatusCode.OK)
                {
                    var parsedResponse = response as RepositoryResponseWithModel<PaginatedResultModel<PaginatedResultViewModel>>;
                    model = parsedResponse?.ReturnModel ?? new();
                }
                var result = ConvertToDataTableModel(model, searchModel);
                SetDatatableActions(result);
                var jsonResult = Json(result);
                return jsonResult;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{_controllerName} Search method threw an exception, Message: {ex.Message}");
                return null;
            }
        }

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
                PaginatedResultModel<PaginatedResultViewModel> items = new();
                if (response.Status == System.Net.HttpStatusCode.OK)
                {
                    var parsedResponse = response as RepositoryResponseWithModel<PaginatedResultModel<PaginatedResultViewModel>>;
                    items = parsedResponse?.ReturnModel ?? new();
                }
                var select2List = GetSelect2List(items);
                return Json(new Select2Repository().GetSelect2PagedResult(pageSize, pageNumber, select2List));
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

            return response.OrderBy(m => m.id).ToList();
        }

        #endregion

        #region[Helper Models]
        protected DatatablePaginatedResultModel<T> ConvertToDataTableModel<T>(PaginatedResultModel<T> model, IBaseSearchModel searchModel) where T : new()
        {
            return new DatatablePaginatedResultModel<T>(searchModel.Draw, model._meta.TotalCount, model.Items);
        }

        public virtual ActionResult DataTableIndexView(CrudListViewModel vm)
        {
            return View("~/Views/Shared/Crud/ListView/DataTable/_Index.cshtml", vm);
        }
        public ActionResult DataTableIndexViewForApproval(CrudListViewModel vm)
        {
            return View("~/Views/Shared/Crud/ListView/DataTable/Approval/_Index.cshtml", vm);
        }

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
            return SetUpdateViewModel($"{action} {_controllerName}", model, action, null, updateViewPath);
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
            return new SearchViewModel();
        }

        protected virtual void SetDatatableActions<T>(DatatablePaginatedResultModel<T> result)
        {
            result.ActionsList = new List<DataTableActionViewModel>()
            {
                    new DataTableActionViewModel() {Action="Detail",Title="Detail",Href=$"/{_controllerName}/Detail/Id"},
                    new DataTableActionViewModel() {Action="Update",Title="Update",Href=$"/{_controllerName}/Update/Id"},
                    new DataTableActionViewModel() {Action="Delete",Title="Delete",Href=$"/{_controllerName}/Delete/Id"},
                };
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

        #endregion


        #region[Abstract Methods]
        public abstract List<DataTableViewModel> GetColumns();
        #endregion
    }
}
