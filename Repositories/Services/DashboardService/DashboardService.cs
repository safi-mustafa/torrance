using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Enums;
using Helpers.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.OverrideLogs;
using Models.TimeOnTools;
using Models.WeldingRodRecord;
using ViewModels.Dashboard;
using ViewModels.TimeOnTools.TOTLog;
using ViewModels.WeldingRodRecord.WRRLog;

namespace Repositories.Services.DashboardService
{
    public class DashboardService : IDashboardService
    {
        private readonly ToranceContext _db;
        private readonly ILogger<DashboardService> _logger;
        private readonly IRepositoryResponse _response;

        public DashboardService(ToranceContext db, ILogger<DashboardService> logger, IRepositoryResponse response)
        {
            _db = db;
            _logger = logger;
            _response = response;
        }



        public async Task<TOTPieChartViewModel> GetTotChartsData(TOTLogSearchViewModel search)
        {
            var model = new TOTPieChartViewModel();
            var totHours = await GetFilteredTOTLogs(search).IgnoreQueryFilters().SumAsync(x => x.ManHours);
            model.Shift = await GetFilteredTOTLogs(search).IgnoreQueryFilters()
              .Include(x => x.Shift)
              .GroupBy(x => x.ShiftId).Select(x => new ChartViewModel
              {
                  Category = x.Max(y => y.Shift.Name),
                  Value = (double)(x.Sum(y => y.ManHours) ?? 0)
                  //Value = (double)((x.Sum(y => y.ManHours) * 100 / totHours) ?? 0)
              }).Take(10).ToListAsync();

            model.Department = await GetFilteredTOTLogs(search).IgnoreQueryFilters()
              .Include(x => x.Department)
              .GroupBy(x => x.DepartmentId).Select(x => new ChartViewModel
              {
                  Category = x.Max(y => y.Department.Name),
                  Value = (double)(x.Sum(y => y.ManHours) ?? 0)
                  //Value = (double)((x.Sum(y => y.ManHours) * 100 / totHours) ?? 0)
              }).Take(10).ToListAsync();

            model.Unit = await GetFilteredTOTLogs(search).IgnoreQueryFilters()
              .Include(x => x.Unit)
              .GroupBy(x => x.UnitId).Select(x => new ChartViewModel
              {
                  Category = x.Max(y => y.Unit.Name),
                  Value = (double)(x.Sum(y => y.ManHours) ?? 0)
                  //Value = (double)((x.Sum(y => y.ManHours) * 100 / totHours) ?? 0)
              }).Take(10).ToListAsync();

            model.RequestReason = await GetFilteredTOTLogs(search).IgnoreQueryFilters()
              .Include(x => x.DelayType)
              .GroupBy(x => x.DelayTypeId).Select(x => new ChartViewModel
              {
                  Category = x.Max(y => y.DelayType.Name),
                  Value = (double)(x.Sum(y => y.ManHours) ?? 0)
                  //Value = (double)((x.Sum(y => y.ManHours) * 100 / totHours) ?? 0)
              }).Take(10).ToListAsync();

            await GetTOTDelayTypeCharts(search, model, totHours);
            return model;

        }

        private async Task GetTOTDelayTypeCharts(TOTLogSearchViewModel search, TOTWorkDelayTypeChartViewModel model, long? totHours)
        {
            model.ShiftDelay = await GetFilteredTOTLogs(search).IgnoreQueryFilters()
                                      .Include(x => x.ShiftDelay)
                                      .GroupBy(x => x.ShiftDelayId).Select(x => new ChartViewModel
                                      {
                                          Category = x.Max(y => y.ShiftDelay.Name) ?? "None",
                                          Value = (double)(x.Sum(y => y.ManHours) ?? 0)
                                          //Value = (double)((x.Sum(y => y.ManHours) * 100 / totHours) ?? 0)
                                      }).Take(10).ToListAsync();

            model.ReworkDelay = await GetFilteredTOTLogs(search).IgnoreQueryFilters()
                                                 .Include(x => x.ReworkDelay)
                                                 .GroupBy(x => x.ReworkDelayId).Select(x => new ChartViewModel
                                                 {
                                                     Category = x.Max(y => y.ReworkDelay.Name) ?? "None",
                                                     Value = (double)(x.Sum(y => y.ManHours) ?? 0)
                                                     //Value = (double)((x.Sum(y => y.ManHours) * 100 / totHours) ?? 0)
                                                 }).Take(10).ToListAsync();

            model.StartOfWorkDelay = await GetFilteredTOTLogs(search).IgnoreQueryFilters()
                                     .Include(x => x.StartOfWorkDelay)
                                     .GroupBy(x => x.StartOfWorkDelayId).Select(x => new ChartViewModel
                                     {
                                         Category = x.Max(y => y.StartOfWorkDelay.Name) ?? "None",
                                         Value = (double)(x.Sum(y => y.ManHours) ?? 0)
                                         //Value = (double)((x.Sum(y => y.ManHours) * 100 / totHours) ?? 0)
                                     }).Take(10).ToListAsync();

            model.OngoingWorkDelay = await GetFilteredTOTLogs(search).IgnoreQueryFilters()
                                    .Include(x => x.OngoingWorkDelay)
                                    .GroupBy(x => x.OngoingWorkDelayId).Select(x => new ChartViewModel
                                    {
                                        Category = x.Max(y => y.OngoingWorkDelay.Name) ?? "None",
                                        Value = (double)(x.Sum(y => y.ManHours) ?? 0)
                                        //Value = (double)((x.Sum(y => y.ManHours) * 100 / totHours) ?? 0)
                                    }).Take(10).ToListAsync();
        }

        public async Task<OverridePieChartViewModel> GetOverrideChartsData(TOTLogSearchViewModel search)
        {
            var model = new OverridePieChartViewModel();
            var totCost = await GetFilteredORLogs(search).IgnoreQueryFilters().SumAsync(x => x.TotalCost);
            model.Shift = await GetFilteredORLogs(search).IgnoreQueryFilters()
              .Include(x => x.Shift)
              .GroupBy(x => x.ShiftId).Select(x => new ChartViewModel
              {
                  Category = x.Max(y => y.Shift.Name),
                  Value = (double)(x.Sum(y => y.TotalCost))
                  //Value = x.Sum(y => y.TotalCost) * 100 / totCost
              }).Take(10).ToListAsync();

            model.Department = await GetFilteredORLogs(search).IgnoreQueryFilters()
              .Include(x => x.Department)
              .GroupBy(x => x.DepartmentId).Select(x => new ChartViewModel
              {
                  Category = x.Max(y => y.Department.Name),
                  Value = (double)(x.Sum(y => y.TotalCost))
                  //Value = x.Sum(y => y.TotalCost) * 100 / totCost
              }).Take(10).ToListAsync();

            model.Unit = await GetFilteredORLogs(search).IgnoreQueryFilters()
              .Include(x => x.Unit)
              .GroupBy(x => x.UnitId).Select(x => new ChartViewModel
              {
                  Category = x.Max(y => y.Unit.Name),
                  Value = (double)(x.Sum(y => y.TotalCost))
                  //Value = x.Sum(y => y.TotalCost) * 100 / totCost
              }).Take(10).ToListAsync();

            model.RequestReason = await GetFilteredORLogs(search).IgnoreQueryFilters()
              .Include(x => x.ReasonForRequest)
              .GroupBy(x => x.ReasonForRequestId).Select(x => new ChartViewModel
              {
                  Category = x.Max(y => y.ReasonForRequest.Name),
                  Value = (double)(x.Sum(y => y.TotalCost))
                  //Value = x.Sum(y => y.TotalCost) * 100 / totCost
              }).Take(10).ToListAsync();

            return model;

        }



        public async Task<StatusChartViewModel> GetTotStatusChartData(TOTLogSearchViewModel search)
        {
            var model = new StatusChartViewModel();
            model.ChartData = await GetFilteredTOTLogs(search).IgnoreQueryFilters()
              .GroupBy(x => x.Status).Select(x => new ChartViewModel
              {
                  Category = x.Max(y => y.Status).ToString(),
                  Value = x.Count()
              }).Take(10).ToListAsync();
            SetDisplayNameForStatus(model.ChartData);
            return model;
        }

        public async Task GetShiftDelayTOTCount(TOTLogSearchViewModel search)
        {
            var model = await GetFilteredTOTLogs(search).IgnoreQueryFilters()
              .GroupBy(x => x.StartOfWorkDelayId).Select(x => new
              {
                  DelayTypeName = x.Max(y => y.DelayType.Name).ToString(),
                  StartOfWorkDelayName = x.Max(y => y.StartOfWorkDelay.Name),
                  StartOfWorkDelay = x.Key,
                  Value = x.Count()
              }).Take(10).ToListAsync();
        }


        public async Task<StatusChartViewModel> GetWrrStatusChartData(WRRLogSearchViewModel search)
        {
            var model = new StatusChartViewModel();
            model.ChartData = await GetFilteredWrrLogs(search).IgnoreQueryFilters()
              .GroupBy(x => x.Status).Select(x => new ChartViewModel
              {
                  Category = x.Max(y => y.Status).ToString(),
                  Value = x.Count()
              }).ToListAsync();
            SetDisplayNameForStatus(model.ChartData);
            return model;

        }

        public async Task<StatusChartViewModel> GetOverrideStatusChartData(TOTLogSearchViewModel search)
        {
            var model = new StatusChartViewModel();
            model.ChartData = await GetFilteredORLogs(search).IgnoreQueryFilters()
              .GroupBy(x => x.Status).Select(x => new ChartViewModel
              {
                  Category = x.Max(y => y.Status).ToString(),
                  Value = x.Count()
              }).ToListAsync();
            SetDisplayNameForStatus(model.ChartData);

            return model;

        }


        public async Task<DashboardViewModel> GetDashboardData()
        {
            try
            {
                var dashboardData = new DashboardViewModel
                {
                    TotalORLogs = await _db.OverrideLogs.Where(x => x.IsDeleted == false && x.IsArchived == false).CountAsync(),
                    TotalWRRLogs = await _db.WRRLogs.Where(x => x.IsDeleted == false && x.IsArchived == false).CountAsync(),
                    TotalTotLogs = await _db.TOTLogs.Where(x => x.IsDeleted == false && x.IsArchived == false).CountAsync()
                };
                return dashboardData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetDashboardData() threw the following exception");
                return new DashboardViewModel();
            }
        }

        private IQueryable<TOTLog> GetFilteredTOTLogs(TOTLogSearchViewModel search)
        {
            return _db.TOTLogs.Where(x =>
                    x.IsDeleted == false
                    &&
                    x.IsArchived == false
                    &&
                    (search.DelayType.Id == null || search.DelayType.Id == x.DelayTypeId)
                    &&
                    (search.Unit.Id == null || search.Unit.Id == x.UnitId)
            );
        }

        private IQueryable<OverrideLog> GetFilteredORLogs(TOTLogSearchViewModel search)
        {
            return _db.OverrideLogs.Where(x =>
                    x.IsDeleted == false
                    &&
                    x.IsArchived == false
                    &&
                    search.Unit.Id == null || search.Unit.Id == 0 || search.Unit.Id == x.UnitId
                );
        }

        private IQueryable<WRRLog> GetFilteredWrrLogs(WRRLogSearchViewModel search)
        {
            return _db.WRRLogs.Where(x =>
                    x.IsDeleted == false
                    &&
                    x.IsArchived == false
                    &&
                    (search.Department.Id == null || search.Department.Id == 0 || search.Department.Id == x.DepartmentId)
                    &&
                    (search.Unit.Id == null || search.Unit.Id == 0 || search.Unit.Id == x.UnitId)
                );
        }

        public async Task<IRepositoryResponse> GetAPITotChartsData(TOTLogSearchViewModel search)
        {
            try
            {
                var model = await GetTotChartsData(search);
                var response = new RepositoryResponseWithModel<TOTPieChartViewModel> { ReturnModel = model };
                return response;
            }
            catch (Exception ex)
            {

            }
            return Response.BadRequestResponse(_response);
        }

        public async Task<IRepositoryResponse> GetAPIOverrideChartsData(TOTLogSearchViewModel search)
        {
            try
            {
                var model = await GetOverrideChartsData(search);
                var response = new RepositoryResponseWithModel<OverridePieChartViewModel> { ReturnModel = model };
                return response;
            }
            catch (Exception ex)
            {

            }
            return Response.BadRequestResponse(_response);
        }

        public async Task<IRepositoryResponse> GetAPIBarChartsData()
        {
            try
            {
                var model = new APIBarChartsVM();
                model.TOTLogs = await GetTotStatusChartData(new TOTLogSearchViewModel());
                model.ORLogs = await GetOverrideStatusChartData(new TOTLogSearchViewModel());
                model.WRRLogs = await GetWrrStatusChartData(new WRRLogSearchViewModel());
                var response = new RepositoryResponseWithModel<APIBarChartsVM> { ReturnModel = model };
                return response;
            }
            catch (Exception ex)
            {

            }
            return Response.BadRequestResponse(_response);
        }

        private void SetDisplayNameForStatus(List<ChartViewModel> data)
        {
            data.ForEach(x =>
            {
                if (Status.TryParse(x.Category, out Status enumValue))
                {
                    x.Category = enumValue.GetDisplayName();
                }
            });
        }
    }

    public class APIBarChartsVM 
    {
        public StatusChartViewModel TOTLogs { get; set; }
        public StatusChartViewModel ORLogs { get; set; }
        public StatusChartViewModel WRRLogs { get; set; }
    }
}
