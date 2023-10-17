using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using NuGet.Packaging;
using Pagination;
using Repositories.Services.TimeOnToolServices.TOTLogService;
using Repositories.Shared.Interfaces;
using Repositories.Shared.UserInfoServices;
using Select2;
using Select2.Model;
using System.Security.Claims;
using ViewModels;
using ViewModels.Common.Company;
using ViewModels.CRUD;
using ViewModels.DataTable;
using ViewModels.OverrideLogs.ORLog;
using ViewModels.TimeOnTools.TOTLog;
using ViewModels.WeldingRodRecord.WRRLog;

namespace Web.Controllers
{
    [Authorize]
    [Authorize(Roles = "SuperAdmin,Administrator,Approver,Employee")]
    public class TOTLogController : ApproveBaseController<ITOTLogService<TOTLogModifyViewModel, TOTLogModifyViewModel, TOTLogDetailViewModel>, TOTLogModifyViewModel, TOTLogModifyViewModel, TOTLogDetailViewModel, TOTLogDetailViewModel, TOTLogSearchViewModel>
    {
        private readonly ITOTLogService<TOTLogModifyViewModel, TOTLogModifyViewModel, TOTLogDetailViewModel> _TOTLogService;
        private readonly ILogger<TOTLogController> _logger;
        private readonly string _loggedInUserRole;
        private readonly IUserInfoService _userInfo;
        private readonly UserManager<ToranceUser> _userManager;
        private readonly IBaseApprove _baseApprove;

        public TOTLogController(ITOTLogService<TOTLogModifyViewModel, TOTLogModifyViewModel, TOTLogDetailViewModel> TOTLogService, ILogger<TOTLogController> logger, IMapper mapper, IUserInfoService userInfo, UserManager<ToranceUser> userManager) : base(TOTLogService, logger, mapper, "TOTLog", "Time On Tool Log", userInfo)
        {
            _TOTLogService = TOTLogService;
            _logger = logger;
            _userInfo = userInfo;
            _userManager = userManager;
            _loggedInUserRole = _userInfo.LoggedInUserRole();
        }

        protected override CrudListViewModel OverrideCrudListVM(CrudListViewModel vm)
        {
            vm.IsExcelDownloadAjaxBased = true;
            return base.OverrideCrudListVM(vm);
        }

        protected override TOTLogSearchViewModel SetDefaultFilters()
        {
            var filters = base.SetDefaultFilters();
            filters.StatusNot.Add(Status.Pending);
            return filters;
        }
        public override List<DataTableViewModel> GetColumns()
        {
            var dataColumns = new List<DataTableViewModel>();
            dataColumns.AddRange(new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "<input type='checkbox' class='select-all-checkbox' onclick='selectAllCheckBoxChanged(this)'>",className="text-right exclude-from-export", data = ""},//
                new DataTableViewModel{title = "Status",data = "FormattedStatusForView",format="html",formatValue="status",exportColumn="FormattedStatus"},
                new DataTableViewModel{title = "Submitted",data = "FormattedCreatedOn", sortingColumn ="CreatedOn", orderable = true},
                new DataTableViewModel{title = "Requestor",data = "Employee.Name",sortingColumn="Employee.FullName", orderable=true},
                new DataTableViewModel{title = "Approver",data = "Approver.Name",sortingColumn="Approver.FullName", orderable=true},
                new DataTableViewModel{title = "Department",data = "Department.Name",className="exclude-form-table include-in-export"},
                new DataTableViewModel{title = "Unit",data = "Unit.Name", orderable=true},
                new DataTableViewModel{title = "Shift",data = "Shift.Name", orderable=true},
                new DataTableViewModel{title = "Permit No",data = "PermitNo", orderable=true},
                new DataTableViewModel{title = "Permit",data = "PermitType.Name", orderable=true},
                new DataTableViewModel{title = "Description",data = "JobDescription",className="exclude-form-table include-in-export"},
                new DataTableViewModel{title = "Company",data = "Company.Name",className="exclude-form-table include-in-export"},
                new DataTableViewModel{title = "Foreman",data = "Foreman",className="exclude-form-table include-in-export"},
                new DataTableViewModel{title = "TWR",data = "Twr",className="exclude-form-table include-in-export"},
                //new DataTableViewModel{title = "Equip.",data = "EquipmentNo", orderable=true},
                //new DataTableViewModel{title = "Delay Type",data = "DelayType.Name", orderable=true},
                new DataTableViewModel{title = "Delay Type",data = "DelayType.Name", orderable=true},
                new DataTableViewModel{title = "Delay Reason",data = "DelayReason",className="exclude-form-table include-in-export"},
                //new DataTableViewModel{title = "Date",data = "FormattedStartOfWork", sortingColumn="StartOfWork", orderable=true},
                new DataTableViewModel{title = "HC",data = "ManPowerAffected", orderable=true},
                new DataTableViewModel{title = "MH",data = "ManHours", orderable=true},
                new DataTableViewModel{title = "TH",data = "TotalHours", orderable=false},
                new DataTableViewModel{title = "Delay Description",data = "DelayDescription",className="exclude-form-table include-in-export"},

                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-from-export"}
            });
            return dataColumns;
        }
        public override async Task<ActionResult> Create()
        {
            var model = await GetCreateViewModel();
            return UpdateView(GetUpdateViewModel("Create", model));
        }
        public override Task<ActionResult> Create(TOTLogModifyViewModel model)
        {
            if(User.IsInRole("Employee") || User.IsInRole("Approver"))
            {
                ModelState.Remove("Employee.Id");
                ModelState.Remove("Employee.Name");
            }
            if (User.IsInRole("Employee"))
            {
                ModelState.Remove("Company.Id");
                ModelState.Remove("Company.Name");
            }
            model.Validate(ModelState);
            return base.Create(model);
        }

        public override Task<ActionResult> Update(TOTLogModifyViewModel model)
        {
            if (User.IsInRole("Employee"))
            {
                ModelState.Remove("Employee.Id");
                ModelState.Remove("Employee.Name");
            }
            model.Validate(ModelState);
            return base.Update(model);
        }

        public override ActionResult DataTableIndexView(CrudListViewModel vm)
        {
            return View("~/Views/TOTLog/_Index.cshtml", vm);
        }

        protected override void SetDatatableActions<T>(DatatablePaginatedResultModel<T> result)
        {
            result.ActionsList = new List<DataTableActionViewModel>()
            {
                    new DataTableActionViewModel() {Action="Detail",Title="Detail",Href=$"/TOTLog/Detail/Id"},
            };
            if (_loggedInUserRole == RolesCatalog.Employee.ToString() || _loggedInUserRole == RolesCatalog.CompanyManager.ToString())
            {
                result.ActionsList.Add(new DataTableActionViewModel() { Action = "Update", Title = "Update", Href = $"/TOTLog/Update/Id", HideBasedOn = "IsEditRestricted" });
            }
            else if (User.IsInRole("SuperAdmin") || User.IsInRole("Administrator"))
            {
                result.ActionsList.Add(new DataTableActionViewModel() { Action = "Update", Title = "Update", Href = $"/TOTLog/Update/Id" });
            }
            result.ActionsList.Add(new DataTableActionViewModel() { Action = "Delete", Title = "Delete", Href = $"/TOTLog/Delete/Id" });
        }

        public async Task<JsonResult> GetTWRNumericValues(string prefix, int pageSize, int pageNumber, string customParams)
        {
            try
            {
                var svm = SetSelect2CustomParams(customParams);
                svm.PerPage = pageSize;
                svm.CalculateTotal = true;
                svm.CurrentPage = pageNumber;
                svm.DisablePagination = true;
                svm.Search = new DataTableSearchViewModel() { value = prefix };
                var response = await _TOTLogService.GetTWRNumericValues<Select2ViewModel>(svm);
                PaginatedResultModel<Select2ViewModel> items = new();
                var totalCount = 0;
                if (response.Status == System.Net.HttpStatusCode.OK)
                {
                    var parsedResponse = response as RepositoryResponseWithModel<PaginatedResultModel<Select2ViewModel>>;
                    items = parsedResponse?.ReturnModel ?? new();
                    totalCount = parsedResponse.ReturnModel._meta.TotalCount;
                }
                if (svm.DisablePagination)
                {
                    pageSize = items.Items.Count;
                }
                return Json(new Select2Repository().GetSelect2PagedResult(pageSize, pageNumber, totalCount, items.Items.ToList()));
            }
            catch (Exception ex)
            {
                _logger.LogError($"TOTLog Select2 method threw an exception, Message: {ex.Message}");
                return null;
            }


        }

        public async Task<JsonResult> GetTWRAlphabeticValues(string prefix, int pageSize, int pageNumber, string customParams)
        {
            try
            {
                var svm = SetSelect2CustomParams(customParams);
                svm.PerPage = pageSize;
                svm.CalculateTotal = true;
                svm.CurrentPage = pageNumber;
                svm.DisablePagination = true;
                svm.Search = new DataTableSearchViewModel() { value = prefix };
                var response = await _TOTLogService.GetTWRAphabeticValues<Select2ViewModel>(svm);
                PaginatedResultModel<Select2ViewModel> items = new();
                var totalCount = 0;
                if (response.Status == System.Net.HttpStatusCode.OK)
                {
                    var parsedResponse = response as RepositoryResponseWithModel<PaginatedResultModel<Select2ViewModel>>;
                    items = parsedResponse?.ReturnModel ?? new();
                    totalCount = parsedResponse.ReturnModel._meta.TotalCount;
                }
                if (svm.DisablePagination)
                {
                    pageSize = items.Items.Count;
                }
                return Json(new Select2Repository().GetSelect2PagedResult(pageSize, pageNumber, totalCount, items.Items.ToList()));
            }
            catch (Exception ex)
            {
                _logger.LogError($"TOTLog Select2 method threw an exception, Message: {ex.Message}");
                return null;
            }


        }

        private async Task<TOTLogModifyViewModel> GetCreateViewModel()
        {
            var model = new TOTLogModifyViewModel();
            var user = await _userManager.GetUserAsync(User);
            if (await _userManager.IsInRoleAsync(user, RolesCatalog.Employee.ToString()))
            {
                model.Company = new CompanyBriefViewModel();
                var companyIdClaim = User.Claims.Where(c => c.Type == "CompanyId").Select(x => x.Value).FirstOrDefault();
                if (companyIdClaim != null)
                {
                    model.Company.Id = int.Parse(companyIdClaim.ToString());
                }
                var companyNameClaim = User.Claims.Where(c => c.Type == "CompanyName").Select(x => x.Value).FirstOrDefault();
                if (companyNameClaim != null)
                {
                    model.Company.Name = companyNameClaim.ToString();
                }
            }
            return model;
        }

        public async Task<IActionResult> DownloadExcel(TOTLogSearchViewModel searchModel)
        {
            var workBook = await _TOTLogService.DownloadExcel(searchModel);
            // Convert the workbook to a byte array
            byte[] fileBytes;
            using (var stream = new MemoryStream())
            {
                workBook.SaveAs(stream);
                fileBytes = stream.ToArray();
            }

            // Set the content type and file name for the Excel file
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "TimeOnTool Logs - Torrance.xlsx";

            // Return the Excel file as a FileStreamResult
            return File(new MemoryStream(fileBytes), contentType, fileName);
        }
    }

}
