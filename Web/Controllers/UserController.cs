using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pagination;
using Repositories.Services.TimeOnToolServices.UserService;
using Select2;
using Select2.Model;
using ViewModels.Authentication;
using ViewModels.Authentication.Approver;
using ViewModels.WeldingRodRecord.Employee;

namespace Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _service;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService service, ILogger<UserController> logger, IMapper mapper)
        {
            _service = service;
            _logger = logger;
        }
        protected UserSearchViewModel SetSelect2CustomParams(string customParams)
        {
            var svm = JsonConvert.DeserializeObject<UserSearchViewModel>(customParams);
            return svm;
        }
        public async Task<JsonResult> Select2(string prefix, int pageSize, int pageNumber, string customParams)
        {
            try
            {
                var svm = SetSelect2CustomParams(customParams);
                svm.PerPage = pageSize;
                svm.CalculateTotal = true;
                svm.CurrentPage = pageNumber;
                svm.Search = new DataTableSearchViewModel() { value = prefix };
                var items = await _service.GetUsers(svm);
                var select2List = GetSelect2List(items);
                return Json(new Select2Repository().GetSelect2PagedResult(pageSize, pageNumber, select2List));
            }
            catch (Exception ex)
            {
                _logger.LogError($"User Select2 method threw an exception, Message: {ex.Message}");
                return null;
            }
        }

        public virtual List<Select2OptionModel<ISelect2Data>> GetSelect2List(PaginatedResultModel<UserBriefViewModel> paginatedResult)
        {
            List<Select2OptionModel<ISelect2Data>> response = new List<Select2OptionModel<ISelect2Data>>();
            foreach (var item in paginatedResult.Items)
            {
                response.Add(new Select2OptionModel<ISelect2Data>
                {
                    id = item.Id.ToString(),
                    text = item.Name
                });
            }

            return response.OrderBy(m => m.id).ToList();
        }
    }
}
