﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pagination;
using Select2;
using Select2.Model;
using ViewModels.Shared;
using Models.Common.Interfaces;
using Enums;
using ViewModels.Authentication.User;
using Repositories.Services.CommonServices.UserService;
using Centangle.Common.ResponseHelpers.Models;
using ViewModels.DataTable;
using ViewModels.Authentication;

namespace Web.Controllers
{
    [Authorize]
    public class UserController<CreateViewModel, UpdateViewModel, DetailViewModel, PaginatedViewModel, SearchViewModel> : CrudBaseController<CreateViewModel, UpdateViewModel, DetailViewModel, PaginatedViewModel, SearchViewModel>
        where DetailViewModel : UserDetailViewModel, IBaseCrudViewModel, new()
        where PaginatedViewModel : class, new()
        where CreateViewModel : UserUpdateViewModel, IBaseCrudViewModel, new()
        where UpdateViewModel : UserUpdateViewModel, IBaseCrudViewModel, IIdentitifier, new()
        where SearchViewModel : class, IBaseSearchModel, new()
    {
        private readonly IUserService<CreateViewModel, UpdateViewModel, DetailViewModel> _service;
        private readonly ILogger<UserController<CreateViewModel, UpdateViewModel, DetailViewModel, PaginatedViewModel, SearchViewModel>> _logger;
        private readonly string _controllerName;
        private readonly RolesCatalog _role;

        public UserController(IUserService<CreateViewModel, UpdateViewModel, DetailViewModel> service, ILogger<UserController<CreateViewModel, UpdateViewModel, DetailViewModel, PaginatedViewModel, SearchViewModel>> logger, IMapper mapper, string controllerName, string title, RolesCatalog role) : base(service, logger, mapper, controllerName, title)
        {
            _service = service;
            _logger = logger;
            _controllerName = controllerName;
            _role = role;
        }

        protected override void SetDatatableActions<T>(DatatablePaginatedResultModel<T> result)
        {
            if (User.IsInRole("Approver") || User.IsInRole("SuperAdmin") || User.IsInRole("Administrator"))
            {
                result.ActionsList = new List<DataTableActionViewModel>()
                {
                    new DataTableActionViewModel() {Action="ResetPassword",Title="ResetPassword",Href=$"/{_controllerName}/ResetPassword/Id"},
                };
            }
            if (User.IsInRole("SuperAdmin") || User.IsInRole("Administrator"))
            {
                result.ActionsList.AddRange(new List<DataTableActionViewModel>()
                {
                    new DataTableActionViewModel() {Action="Detail",Title="Detail",Href=$"/{_controllerName}/Detail/Id"},
                    new DataTableActionViewModel() {Action="Update",Title="Update",Href=$"/{_controllerName}/Update/Id"},
                    new DataTableActionViewModel() {Action="Delete",Title="Delete",Href=$"/{_controllerName}/Delete/Id"},
                });
            }

        }
        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Full Name",data = "FullName"},
                new DataTableViewModel{title = "Company",data = "Company.Name"},
                new DataTableViewModel{title = "Email",data = "Email"},
                new DataTableViewModel{title = "Access Code",data = "FormattedAccessCode"},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-form-export"}

            };
        }
        protected SearchViewModel SetSelect2CustomParams(string customParams)
        {
            if (customParams == null)
                return new SearchViewModel();
            var svm = JsonConvert.DeserializeObject<SearchViewModel>(customParams);
            return svm;
        }
        public override async Task<JsonResult> Select2(string prefix, int pageSize, int pageNumber, string customParams)
        {
            try
            {
                var svm = SetSelect2CustomParams(customParams);
                svm.PerPage = pageSize;
                svm.CalculateTotal = true;
                svm.CurrentPage = pageNumber;
                svm.Search = new DataTableSearchViewModel() { value = prefix };
                var response = await _service.GetAll<PaginatedViewModel>(svm);
                var parsedResponse = response as RepositoryResponseWithModel<PaginatedResultModel<PaginatedViewModel>>;
                var totalCount = parsedResponse.ReturnModel._meta.TotalCount;
                var select2List = GetSelect2List(parsedResponse.ReturnModel);
                return Json(new Select2Repository().GetSelect2PagedResult(pageSize, pageNumber, totalCount, select2List));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{_controllerName} Select2 method threw an exception, Message: {ex.Message}");
                return null;
            }
        }
        public override List<Select2OptionModel<ISelect2Data>> GetSelect2List(PaginatedResultModel<PaginatedViewModel> paginatedResult)
        {
            List<Select2OptionModel<ISelect2Data>> response = new List<Select2OptionModel<ISelect2Data>>();
            var paginatedItems = (paginatedResult as PaginatedResultModel<DetailViewModel>);
            foreach (var item in paginatedItems.Items)
            {
                response.Add(new Select2OptionModel<ISelect2Data>
                {
                    id = item.Id.ToString(),
                    text = item.Name,
                    additionalAttributesModel = item
                });
            }
            return response.OrderBy(m => m.id).ToList();
        }
        public override async Task<ActionResult> Create(CreateViewModel model)
        {
            bool isUnique = await IsAccessCodeUnique(model);
            if (!isUnique)
            {
                ModelState.AddModelError("AccessCode", "Access Code already in use.");
            }
            return await base.Create(model);
        }
        public virtual async Task<bool> IsAccessCodeUnique(CreateViewModel model)
        {
            return await _service.IsAccessCodeUnique(model.Id, model.AccessCode);

        }
        public override async Task<ActionResult> Update(UpdateViewModel model)
        {
            ModelState.Remove("Email");
            ModelState.Remove("AccessCode");
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            return await base.Update(model);
        }
        public async Task<IActionResult> ValidateAccessCode(int id, string accessCode)
        {
            return Json(await _service.IsAccessCodeUnique(id, accessCode));
        }
        public async Task<IActionResult> ValidateEmail(int id, string email)
        {
            return Json(await _service.IsEmailUnique(id, email));
        }

        public async Task<ActionResult> ResetPassword(int id)
        {
            try
            {
                var response = await _service.GetById(id);
                if (response.Status == System.Net.HttpStatusCode.OK)
                {
                    var parsedResponse = response as RepositoryResponseWithModel<UserDetailViewModel>;
                    var model = parsedResponse?.ReturnModel ?? new();
                    ChangeAccessCodeVM viewModel = new ChangeAccessCodeVM
                    {
                        Id = model.Id ?? 0,
                        CurrentAccessCode = model.AccessCode
                    };
                    return View(viewModel);
                }
                else
                {
                    _logger.LogInformation($"Record with id " + id + "not found");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex) { _logger.LogError($"Account ResetPassword method threw an exception, Message: {ex.Message}"); return RedirectToAction("Index"); }
        }
        [HttpPost]
        public async Task<ActionResult> ResetPassword(ChangeAccessCodeVM model)
        {
            await _service.ResetAccessCode(model);
            return RedirectToAction("Index");
        }
    }
}
