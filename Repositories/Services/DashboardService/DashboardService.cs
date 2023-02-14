using DataLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.OverrideLogs;
using Models.TimeOnTools;
using Models.WeldingRodRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Dashboard;
using ViewModels.TimeOnTools.TOTLog;
using ViewModels.WeldingRodRecord.WRRLog;

namespace Repositories.Services.DashboardService
{
    public class DashboardService : IDashboardService
    {
        private readonly ToranceContext _db;
        private readonly ILogger<DashboardService> _logger;

        public DashboardService(ToranceContext db, ILogger<DashboardService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<TOTPieChartViewModel> GetTotChartsData(TOTLogSearchViewModel search)
        {
            var model = new TOTPieChartViewModel();
            var totHours = await GetFilteredTOTLogs(search).IgnoreQueryFilters().SumAsync(x => x.ManHours);
            model.Shift = await GetFilteredTOTLogs(search).IgnoreQueryFilters()
              .Include(x => x.Shift)
              .GroupBy(x => x.ShiftId).Select(x => new LogPieChartViewModel
              {
                  Category = x.Max(y => y.Shift.Name),
                  Value = (double)((x.Sum(y => y.ManHours) * 100 / totHours) ?? 0)
              }).ToListAsync();

            model.Department = await GetFilteredTOTLogs(search).IgnoreQueryFilters()
              .Include(x => x.Department)
              .GroupBy(x => x.DepartmentId).Select(x => new LogPieChartViewModel
              {
                  Category = x.Max(y => y.Department.Name),
                  Value = (double)((x.Sum(y => y.ManHours) * 100 / totHours) ?? 0)
              }).ToListAsync();

            model.Unit = await GetFilteredTOTLogs(search).IgnoreQueryFilters()
              .Include(x => x.Unit)
              .GroupBy(x => x.UnitId).Select(x => new LogPieChartViewModel
              {
                  Category = x.Max(y => y.Unit.Name),
                  Value = (double)((x.Sum(y => y.ManHours) * 100 / totHours) ?? 0)
              }).ToListAsync();

            model.RequestReason = await GetFilteredTOTLogs(search).IgnoreQueryFilters()
              .Include(x => x.ReasonForRequest)
              .GroupBy(x => x.ReasonForRequestId).Select(x => new LogPieChartViewModel
              {
                  Category = x.Max(y => y.ReasonForRequest.Name),
                  Value = (double)((x.Sum(y => y.ManHours) * 100 / totHours) ?? 0)
              }).ToListAsync();

            return model;

        }

        public async Task<OverridePieChartViewModel> GetOverrideChartsData(TOTLogSearchViewModel search)
        {
            var model = new OverridePieChartViewModel();
            var totHours = await GetFilteredORLogs(search).IgnoreQueryFilters().SumAsync(x => x.TotalCost);
            model.Shift = await GetFilteredORLogs(search).IgnoreQueryFilters()
              .Include(x => x.Shift)
              .GroupBy(x => x.ShiftId).Select(x => new LogPieChartViewModel
              {
                  Category = x.Max(y => y.Shift.Name),
                  Value = x.Sum(y => y.TotalCost) * 100 / totHours
              }).ToListAsync();

            model.Department = await GetFilteredORLogs(search).IgnoreQueryFilters()
              .Include(x => x.Department)
              .GroupBy(x => x.DepartmentId).Select(x => new LogPieChartViewModel
              {
                  Category = x.Max(y => y.Department.Name),
                  Value = x.Sum(y => y.TotalCost) * 100 / totHours
              }).ToListAsync();

            model.Unit = await GetFilteredORLogs(search).IgnoreQueryFilters()
              .Include(x => x.Unit)
              .GroupBy(x => x.UnitId).Select(x => new LogPieChartViewModel
              {
                  Category = x.Max(y => y.Unit.Name),
                  Value = x.Sum(y => y.TotalCost) * 100 / totHours
              }).ToListAsync();

            model.RequestReason = await GetFilteredORLogs(search).IgnoreQueryFilters()
              .Include(x => x.ReasonForRequest)
              .GroupBy(x => x.ReasonForRequestId).Select(x => new LogPieChartViewModel
              {
                  Category = x.Max(y => y.ReasonForRequest.Name),
                  Value = x.Sum(y => y.TotalCost) * 100 / totHours
              }).ToListAsync();

            return model;

        }



        public async Task<StatusChartViewModel> GetTotStatusChartData(TOTLogSearchViewModel search)
        {
            var model = new StatusChartViewModel();
            model.ChartData = await GetFilteredTOTLogs(search).IgnoreQueryFilters()
              .GroupBy(x => x.Status).Select(x => new BarChartViewModel
              {
                  Category = x.Max(y => y.Status).ToString(),
                  Value = x.Count()
              }).ToListAsync();

            return model;

        }

        public async Task<StatusChartViewModel> GetWrrStatusChartData(WRRLogSearchViewModel search)
        {
            var model = new StatusChartViewModel();
            model.ChartData = await GetFilteredWrrLogs(search).IgnoreQueryFilters()
              .GroupBy(x => x.Status).Select(x => new BarChartViewModel
              {
                  Category = x.Max(y => y.Status).ToString(),
                  Value = x.Count()
              }).ToListAsync();

            return model;

        }

        public async Task<StatusChartViewModel> GetOverrideStatusChartData(TOTLogSearchViewModel search)
        {
            var model = new StatusChartViewModel();
            model.ChartData = await GetFilteredORLogs(search).IgnoreQueryFilters()
              .GroupBy(x => x.Status).Select(x => new BarChartViewModel
              {
                  Category = x.Max(y => y.Status).ToString(),
                  Value = x.Count()
              }).ToListAsync();

            return model;

        }


        public async Task<DashboardViewModel> GetDashboardData()
        {
            try
            {
                var dashboardData = new DashboardViewModel
                {
                    TotalORLogs = await _db.OverrideLogs.Where(x=>x.IsDeleted==false).CountAsync(),
                    TotalWRRLogs = await _db.WRRLogs.Where(x => x.IsDeleted == false).CountAsync(),
                    TotalTotLogs = await _db.TOTLogs.Where(x => x.IsDeleted == false).CountAsync()
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
                    x.IsDeleted == false &&
                    (search.DelayType.Id == null || search.DelayType.Id == x.DelayTypeId)
                    &&
                    (search.Unit.Id == null || search.Unit.Id == x.UnitId)
            );
        }

        private IQueryable<OverrideLog> GetFilteredORLogs(TOTLogSearchViewModel search)
        {
            return _db.OverrideLogs.Where(x =>

                    x.IsDeleted == false &&
                    search.Unit.Id == null || search.Unit.Id == 0 || search.Unit.Id == x.UnitId
                );
        }

        private IQueryable<WRRLog> GetFilteredWrrLogs(WRRLogSearchViewModel search)
        {
            return _db.WRRLogs.Where(x =>
                    x.IsDeleted == false &&
                    (search.Department.Id == null || search.Department.Id == 0 || search.Department.Id == x.DepartmentId)
                    &&
                    (search.Unit.Id == null || search.Unit.Id == 0 || search.Unit.Id == x.UnitId)
                );
        }


    }
}
