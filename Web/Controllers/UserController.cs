using AutoMapper;
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
using Centangle.Common.ResponseHelpers.Error;
using Microsoft.AspNetCore.Identity;
using Models;
using DocumentFormat.OpenXml.Spreadsheet;

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
        private readonly UserManager<ToranceUser> _userManager;
        private readonly string _controllerName;
        private readonly RolesCatalog _role;

        public UserController(IUserService<CreateViewModel, UpdateViewModel, DetailViewModel> service, ILogger<UserController<CreateViewModel, UpdateViewModel, DetailViewModel, PaginatedViewModel, SearchViewModel>> logger, IMapper mapper, UserManager<ToranceUser> userManager, string controllerName, string title, RolesCatalog role, bool hideCreateButton = false) : base(service, logger, mapper, controllerName, title, hideCreateButton)
        {
            _service = service;
            _logger = logger;
            _userManager = userManager;
            _controllerName = controllerName;
            _role = role;
        }

        protected override void SetDatatableActions<T>(DatatablePaginatedResultModel<T> result)
        {
            result.ActionsList = new List<DataTableActionViewModel>();
            if ((User.IsInRole("SuperAdmin") || User.IsInRole("Administrator")) && (_controllerName != "Requestor"))
            {
                result.ActionsList.Add(
                    new DataTableActionViewModel() { Action = "ResetPassword", Title = "ResetPassword", Href = $"/{_controllerName}/ResetPassword/Id" }
                );
            }
            if (User.IsInRole("Approver") || User.IsInRole("SuperAdmin") || User.IsInRole("Administrator"))
            {
                result.ActionsList.Add(
                    new DataTableActionViewModel() { Action = "ResetAccessCode", Title = "ResetAccessCode", Href = $"/{_controllerName}/ResetAccessCode/Id" }
                );
            }
            if (User.IsInRole("SuperAdmin") || User.IsInRole("Administrator"))
            {
                result.ActionsList.AddRange(new List<DataTableActionViewModel>()
                {
                    new DataTableActionViewModel() { Action = "Update", Title = "Update", Href = $"/{_controllerName}/Update/Id" },                   
                    new DataTableActionViewModel() {Action="Delete",Title="Delete",Href=$"/{_controllerName}/Delete/Id"},
                });
            }
            result.ActionsList.Add(new DataTableActionViewModel() { Action = "Detail", Title = "Detail", Href = $"/{_controllerName}/Detail/Id" });

        }
        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Full Name",data = "FullName", orderable = true},
                new DataTableViewModel{title = "Company",data = "Company.Name", orderable = true},
                new DataTableViewModel{title = "Email",data = "Email", orderable = true},
                new DataTableViewModel{title = "Access Code",data = "FormattedAccessCode", orderable = false},
                new DataTableViewModel{title = "Status",data = "FormattedStatus", orderable = false},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-from-export"}

            };
        }
        protected SearchViewModel SetSelect2CustomParams(string customParams)
        {
            if (customParams == null)
                return new SearchViewModel();
            var svm = JsonConvert.DeserializeObject<SearchViewModel>(customParams);
            return svm;
        }
        [AllowAnonymous]
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
            bool isAccessCodeUnique = await IsAccessCodeUnique(model);

            if (!isAccessCodeUnique)
            {
                ModelState.AddModelError("AccessCode", "Access Code already in use.");
            }
            bool isEmailUnique = await IsEmailUnique(model);
            if (!isEmailUnique)
            {
                ModelState.AddModelError("Email", "Email already in use.");
            }
            if (string.IsNullOrEmpty(model.Password))
            {
                model.Password = "Torrance";
                model.ChangePassword = true;
            }
            return await base.Create(model);
        }
        public virtual async Task<bool> IsAccessCodeUnique(UserUpdateViewModel model)
        {
            return await _service.IsAccessCodeUnique(model.Id, model.AccessCode);

        }
        public virtual async Task<bool> IsEmailUnique(UserUpdateViewModel model)
        {
            return await _service.IsEmailUnique(model.Id, model.Email);

        }

        public override async Task<ActionResult> Update(UpdateViewModel model)
        {
            ModelState.Remove("AccessCode");
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            bool isEmailUnique = await IsEmailUnique(model);
            if (!isEmailUnique)
            {
                ModelState.AddModelError("Email", "Email already in use.");
            }
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
                    var parsedResponse = response as RepositoryResponseWithModel<DetailViewModel>;
                    var model = parsedResponse?.ReturnModel ?? new();
                    ResetPasswordVM viewModel = new ResetPasswordVM
                    {
                        Email = model.Email
                    };
                    return View("Views/User/_ResetPassword.cshtml", viewModel);
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
        public async Task<JsonResult> ResetPassword(ResetPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("Email", "User does not exist.");
                    //return ReturnProcessedResponse(Centangle.Common.ResponseHelpers.Response.BadRequestResponse(_response));
                }
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetResult = await _userManager.ResetPasswordAsync(user, code, model.Password);
                if (resetResult.Succeeded)
                {
                    return new JsonResult(new { success = true });
                }
                else
                {
                    ErrorsHelper.AddErrorsToModelState(resetResult, ModelState, "Password");
                }

            }
            var errors = ModelState.ToDictionary(
             kvp => kvp.Key,
             kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()
            );
            return new JsonResult(new { success = false, errors = errors });
        }


        public async Task<ActionResult> ResetAccessCode(int id)
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
                    return View("Views/User/_ResetAccessCode.cshtml", viewModel);
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
        public async Task<ActionResult> ResetAccessCode(ChangeAccessCodeVM model)
        {
            await _service.ResetAccessCode(model);
            return RedirectToAction("Index");
        }
    }
}
