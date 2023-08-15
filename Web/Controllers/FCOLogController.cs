﻿using AutoMapper;
using Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repositories.Shared.Interfaces;
using Repositories.Shared.UserInfoServices;
using ViewModels.Common.Company;
using ViewModels.CRUD;
using ViewModels.DataTable;
using ViewModels;
using Repositories.Services.AppSettingServices.WRRLogService;
using ViewModels.OverrideLogs.ORLog;

namespace Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator,Approver,Employee")]
    public class FCOLogController : CrudBaseController<FCOLogModifyViewModel, FCOLogModifyViewModel, FCOLogDetailViewModel, FCOLogDetailViewModel, FCOLogSearchViewModel>
    {
        private readonly IFCOLogService<FCOLogModifyViewModel, FCOLogModifyViewModel, FCOLogDetailViewModel> _FCOLogService;
        private readonly ILogger<FCOLogController> _logger;
        private readonly IUserInfoService _userInfo;
        private readonly UserManager<ToranceUser> _userManager;
        private readonly string _loggedInUserRole;
        private readonly IBaseApprove _approveService;
        public FCOLogController(IFCOLogService<FCOLogModifyViewModel, FCOLogModifyViewModel, FCOLogDetailViewModel> FCOLogService, ILogger<FCOLogController> logger, IMapper mapper, IUserInfoService userInfo, UserManager<ToranceUser> userManager) : base(FCOLogService, logger, mapper, "FCOLog", "Field Change Order Logs")
        {
            _FCOLogService = FCOLogService;
            _logger = logger;
            _userInfo = userInfo;
            _userManager = userManager;
            _loggedInUserRole = _userInfo.LoggedInUserRole();
        }
        protected override FCOLogSearchViewModel SetDefaultFilters()
        {
            var filters = base.SetDefaultFilters();
            filters.StatusNot = Enums.Status.Pending;
            return filters;
        }
        protected override CrudListViewModel OverrideCrudListVM(CrudListViewModel vm)
        {
            vm.IsExcelDownloadAjaxBased = true;
            return base.OverrideCrudListVM(vm);
        }
        public override List<DataTableViewModel> GetColumns()
        {
            var dataColumns = new List<DataTableViewModel>();
            dataColumns.AddRange(new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "<input type='checkbox' class='select-all-checkbox' onclick='selectAllCheckBoxChanged(this)'>",className="text-right exclude-from-export", data = ""},//
                new DataTableViewModel{title = "Status",data = "FormattedStatus",format="html",formatValue="status",exportColumn="FormattedStatus"},
                new DataTableViewModel{title = "FCO#",data = "SrNoFormatted",exportColumn="SrNoFormatted"},
                new DataTableViewModel{title = "Type",data = "FCOType.Name", sortingColumn ="FCOType.Name", orderable = true},
                new DataTableViewModel{title = "Reason",data = "FCOReason.Name",sortingColumn="FCOReason.Name", orderable=true},
                new DataTableViewModel{title = "Detail",data = "DescriptionOfFinding",sortingColumn="DescriptionOfFinding", orderable=true},
                new DataTableViewModel{title = "$ Impact",data = "TotalCostFormatted", orderable=true},
                new DataTableViewModel{title = "Issued Date",data = "DateFormatted", orderable=true},
                new DataTableViewModel{title = "Name",data = "Company.Name", orderable=true},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-from-export"}
            });
            return dataColumns;

        }


        public async Task<IActionResult> ValidateFCOLogEmail(int id, string email)
        {
            return Json(await _FCOLogService.IsFCOLogEmailUnique(id, email));
        }

        public override async Task<ActionResult> Create()
        {
            var model = await GetCreateViewModel();
            return UpdateView(GetUpdateViewModel("Create", model));
        }
        [HttpPost]
        public override Task<ActionResult> Create(FCOLogModifyViewModel model)
        {
            if (User.IsInRole("Employee"))
            {
                ModelState.Remove("Employee.Id");
                ModelState.Remove("Employee.Name");
            }
            //model = new FCOLogModifyViewModel
            //{
            //    AdditionalInformation = "alksdjflkajdlkfjaldf",
            //    AnalysisOfAlternatives = true,
            //    Attachment = model.Attachment,
            //    Contractor = model.Contractor,
            //    Date = model.Date,

            //};
            return base.Create(model);
        }
        public override ActionResult DataTableIndexView(CrudListViewModel vm)
        {
            return View("~/Views/FCOLog/_Index.cshtml", vm);
        }

        protected override void SetDatatableActions<T>(DatatablePaginatedResultModel<T> result)
        {
            result.ActionsList = new List<DataTableActionViewModel>()
            {
                    new DataTableActionViewModel() {Action="_GetFCOComments",Title="Comments",Href=$"/FCOLog/_GetFCOComments/Id",Class="@FCOCommentsClass"},
                    new DataTableActionViewModel() {Action="Detail",Title="Detail",Href=$"/FCOLog/Detail/Id"},

            };
            if (_loggedInUserRole == RolesCatalog.Employee.ToString() || _loggedInUserRole == RolesCatalog.CompanyManager.ToString())
            {
                result.ActionsList.Add(new DataTableActionViewModel() { Action = "Update", Title = "Update", Href = $"/FCOLog/Update/Id", HideBasedOn = "IsEditRestricted" });
            }
            result.ActionsList.Add(new DataTableActionViewModel() { Action = "Delete", Title = "Delete", Href = $"/FCOLog/Delete/Id" });
        }

        private async Task<FCOLogModifyViewModel> GetCreateViewModel()
        {
            var model = new FCOLogModifyViewModel();
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

        public async Task<IActionResult> DownloadExcel(FCOLogSearchViewModel searchModel)
        {
            var workBook = await _FCOLogService.DownloadExcel(searchModel);
            // Convert the workbook to a byte array
            byte[] fileBytes;
            using (var stream = new MemoryStream())
            {
                workBook.SaveAs(stream);
                fileBytes = stream.ToArray();
            }

            // Set the content type and file name for the Excel file
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "WeldingRodRecord Logs - Torrance.xlsx";

            // Return the Excel file as a FileStreamResult
            return File(new MemoryStream(fileBytes), contentType, fileName);
        }

        public IActionResult _LabourSectionRow(FCOSectionModifyViewModel model, int rowNumber)
        {
            ViewData["RowNumber"] = rowNumber;
            return PartialView("_LabourSectionRow", model);
        }

        public IActionResult _SectionRow(FCOSectionModifyViewModel model, int rowNumber, string rowClass, string collectionType)
        {
            ViewData["RowNumber"] = rowNumber;
            ViewData["RowClass"] = rowClass;
            ViewData["CollectionType"] = collectionType;
            return PartialView("_SectionRow", model);
        }

        public async Task<IActionResult> _GetFCOComments(long Id)
        {
            var comments = await _FCOLogService.GetFCOComments(Id);
            return View("_Comment", comments);
        }
    }
}
