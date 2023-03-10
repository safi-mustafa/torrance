using System;
using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using Microsoft.AspNetCore.Mvc;
using Models.Common.Interfaces;
using Pagination;
using Repositories.Interfaces;
using ViewModels.CRUD;
using ViewModels.DataTable;
using ViewModels.Shared;

namespace Web.Controllers.SharedControllers
{
    public abstract class DatatableBaseController<PaginatedResultViewModel, SearchViewModel> : Controller

        where PaginatedResultViewModel : class, new()
        where SearchViewModel : IBaseSearchModel, new()
    {
        private readonly IBaseSearch _service;
        private readonly ILogger<Controller> _logger;
        private readonly string _controllerName;
        private readonly string _title;
        private readonly bool _hideCreateButton;
        private readonly bool _useSameUpdateViews;
        private readonly IMapper _mapper;
        public DatatableBaseController(IBaseSearch service, ILogger<Controller> logger, IMapper mapper, string controllerName, string title, bool hideCreateButton = false, bool useSameUpdateViews = true)
        {
            _service = service;
            _logger = logger;
            _controllerName = controllerName;
            _title = title;
            _hideCreateButton = hideCreateButton;
            _useSameUpdateViews = useSameUpdateViews;
            _mapper = mapper;
        }


        public virtual ActionResult Index()
        {
            var vm = new CrudListViewModel();
            vm.Title = _title;
            vm.Filters = SetDefaultFilters();
            vm.DatatableColumns = GetColumns();
            vm.DisableSearch = false;
            vm.HideCreateButton = _hideCreateButton;
            vm.ControllerName = _controllerName;
            vm.DataUrl = $"/{_controllerName}/Search";
            vm.SearchViewPath = $"~/Views/{_controllerName}/_Search.cshtml";
            vm = OverrideCrudListVM(vm);
            return DataTableIndexView(vm);
        }

        protected virtual CrudListViewModel OverrideCrudListVM(CrudListViewModel vm)
        {
            return vm;
        }

        protected virtual SearchViewModel SetDefaultFilters()
        {
            return new SearchViewModel();
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


        protected DatatablePaginatedResultModel<T> ConvertToDataTableModel<T>(PaginatedResultModel<T> model, IBaseSearchModel searchModel) where T : new()
        {
            return new DatatablePaginatedResultModel<T>(searchModel.Draw, model._meta.TotalCount, model.Items);
        }

        public virtual ActionResult DataTableIndexView(CrudListViewModel vm)
        {
            return View("~/Views/Shared/Crud/ListView/DataTable/_Index.cshtml", vm);
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

        public ActionResult DataTableIndexViewForApproval(CrudListViewModel vm)
        {
            return View("~/Views/Shared/Crud/ListView/DataTable/Approval/_Index.cshtml", vm);
        }

        #region[Abstract Methods]
        public abstract List<DataTableViewModel> GetColumns();
        #endregion
    }
}

