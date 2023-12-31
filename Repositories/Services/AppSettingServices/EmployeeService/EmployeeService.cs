﻿using AutoMapper;
using DataLibrary;
using Microsoft.Extensions.Logging;
using Repositories.Shared.AuthenticationService;
using Centangle.Common.ResponseHelpers.Models;
using Models.Common.Interfaces;
using ViewModels.Shared;
using Microsoft.AspNetCore.Identity;
using Models;
using Repositories.Services.CommonServices.UserService;
using ViewModels.WeldingRodRecord.Employee;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Data;
using ViewModels.Common.Company;
using Repositories.Shared.UserInfoServices;
using Pagination;
using Helpers.Extensions;
using Models.WeldingRodRecord;
using Centangle.Common.ResponseHelpers;
using Enums;
using Models.OverrideLogs;
using Models.TimeOnTools;
using System.Linq.Dynamic.Core;
using ExcelReader;

namespace Repositories.Services.AppSettingServices.EmployeeService
{
    public class EmployeeService<CreateViewModel, UpdateViewModel, DetailViewModel> : UserService<CreateViewModel, UpdateViewModel, DetailViewModel>, IEmployeeService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly UserManager<ToranceUser> _userManager;
        private readonly ILogger<EmployeeService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identity;
        private readonly IRepositoryResponse _response;

        public EmployeeService(ToranceContext db, UserManager<ToranceUser> userManager, ILogger<EmployeeService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IMapper mapper, IIdentityService identity, IRepositoryResponse response, IUserInfoService userInfoService)
            :
            base(db, RolesCatalog.Employee, userManager, logger, mapper, identity, response, userInfoService)
        {
            _db = db;
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
            _identity = identity;
            _response = response;
        }


        public List<T> AddModel<T>(List<DataTable> dataLists, int ind, int startInd) where T : new()
        {
            List<T> list = new List<T>();
            try
            {
                for (int i = startInd; i < dataLists[ind].Rows.Count; i++)
                {
                    T val = new T();
                    for (int j = 0; j < dataLists[ind].Columns.Count; j++)
                    {
                        string text = dataLists[ind].Columns[j].ColumnName.ToString();
                        if (!string.IsNullOrEmpty(text))
                        {
                            SetObjectProperty(val, text, dataLists[ind].Rows[i][j].ToString());
                        }
                    }
                    list.Add(val);
                }
                return list;
            }
            catch (Exception)
            {
                return list;
            }
        }
        private void SetObjectProperty(object myObject, string propertyName, string value)
        {
            try
            {
                PropertyInfo[] properties = myObject.GetType().GetProperties();
                PropertyInfo[] array = properties;
                foreach (PropertyInfo propertyInfo in array)
                {
                    if (propertyInfo.PropertyType.IsClass)
                    {
                        var modelProperty = propertyInfo.Name;
                        if (propertyName.Contains('.'))
                        {
                            // var trimmedPropertyName = 
                        }

                    }
                    else if (propertyInfo.Name == propertyName.Trim())
                    {
                        Type propertyType = propertyInfo.PropertyType;
                        if (propertyName == "Company.Name")
                        {
                        }
                        if (propertyType == typeof(float) && value != "")
                        {
                            propertyInfo.SetValue(myObject, float.Parse(value));
                        }
                        if (propertyType == typeof(string))
                        {
                            propertyInfo.SetValue(myObject, value);
                        }
                        if (propertyType == typeof(double) && value != "")
                        {
                            propertyInfo.SetValue(myObject, double.Parse(value));
                        }
                        if (propertyType == typeof(int) && value != "")
                        {
                            propertyInfo.SetValue(myObject, int.Parse(value));
                        }
                        if (propertyType == typeof(long) && value != "")
                        {
                            propertyInfo.SetValue(myObject, long.Parse(value));
                        }
                        if (propertyType == typeof(Guid) && value != "")
                        {
                            propertyInfo.SetValue(myObject, Guid.Parse(value));
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private async Task<bool> ProcessExcelData(List<EmployeeModifyViewModel> excelDataList)
        {
            try
            {
                var existingEmployees = await _db.Users.Select(x => new { Id = x.Id, Email = x.Email }).ToListAsync();
                var companies = await _db.Companies.ToListAsync();
                foreach (var item in excelDataList)
                {
                    var trimmedCompanyName = item.Company.Name.Trim().ToLower();
                    item.Company = new CompanyBriefViewModel { Id = companies.Where(x => x.Name.ToLower().Trim().Equals(trimmedCompanyName)).Select(x => x.Id).FirstOrDefault() };
                    var existingEmployee = existingEmployees.Where(x => x.Email == item.Email.Trim()).FirstOrDefault();
                    if (existingEmployee != null)
                    {
                        item.Id = existingEmployee.Id;
                        await Update(item as UpdateViewModel);
                    }
                    else
                    {
                        await Create(item as CreateViewModel);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async override Task<IRepositoryResponse> GetAll<M>(IBaseSearchModel searchFilter)
        {
            try
            {
                var search = searchFilter as EmployeeSearchViewModel;
                var paginatedDbSet = GetPaginationDbSet(search);
                search.IgnoreOrdering = true;
                var paginatedUserIdResult = await paginatedDbSet.Paginate(search);
                var paginatedUserIds = paginatedUserIdResult.Items.Select(x => x.Id).ToList();
                var result = await _db.Users.Include(x => x.Company).Where(x => paginatedUserIds.Contains(x.Id)).OrderColumns(search).ToListAsync();
                if (result != null)
                {
                    var paginatedResult = new PaginatedResultModel<M>();
                    paginatedResult.Items = _mapper.Map<List<M>>(result.ToList());
                    paginatedResult._meta = paginatedUserIdResult._meta;
                    paginatedResult._links = paginatedUserIdResult._links;
                    var response = new RepositoryResponseWithModel<PaginatedResultModel<M>> { ReturnModel = paginatedResult };
                    return response;
                }
                _logger.LogWarning($"No record found for {typeof(Employee).FullName} in GetAll()");
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetAll() method for {typeof(Employee).FullName} threw an exception.");
                return Response.BadRequestResponse(_response);
            }
        }
        public IQueryable<EmployeeDetailViewModel> GetPaginationDbSet(EmployeeSearchViewModel search)
        {
            var filters = SetQueryFilter(search);

            var empQueryable = (from user in _db.Users.Include(x => x.Company)
                                join userRole in _db.UserRoles on user.Id equals userRole.UserId
                                join r in _db.Roles on userRole.RoleId equals r.Id
                                where r.Name == RolesCatalog.Employee.ToString()
                                select user).Where(filters);
            if (search.IsSearchForm && search.LogType != FilterLogType.None)
            {
                switch (search.LogType)
                {
                    case FilterLogType.Override:
                        empQueryable = JoinEmployeesWithLogs<OverrideLog>(empQueryable); break;
                    case FilterLogType.TimeOnTools:
                        empQueryable = JoinEmployeesWithLogs<TOTLog>(empQueryable); break;
                    case FilterLogType.WeldingRodRecord:
                        empQueryable = JoinEmployeesWithLogs<WRRLog>(empQueryable); break;
                    case FilterLogType.All:
                        return (
                            from ap in empQueryable
                            join tl in _db.TOTLogs on ap.Id equals tl.EmployeeId into ttl
                            from tl in ttl.DefaultIfEmpty()
                            join wl in _db.WRRLogs on ap.Id equals wl.EmployeeId into wwl
                            from wl in wwl.DefaultIfEmpty()
                            join ol in _db.OverrideLogs on ap.Id equals ol.EmployeeId into ool
                            from ol in ool.DefaultIfEmpty()
                            where
                                 (tl != null && tl.Status != Status.Approved)
                                 ||
                                 (wl != null && wl.Status != Status.Approved)
                                 ||
                                 (ol != null && ol.Status != Status.Approved)
                            group ap by ap.Id
                                      ).Select(x => new EmployeeDetailViewModel { Id = x.Key });
                }
            }
            return empQueryable.OrderColumns(search)
                            .Select(x => new EmployeeDetailViewModel { Id = x.Id }).GroupBy(x => x.Id)
                            .Select(x => new EmployeeDetailViewModel { Id = x.Max(m => m.Id) })
                            .AsQueryable();
        }
        private IQueryable<ToranceUser> JoinEmployeesWithLogs<T>(IQueryable<ToranceUser> userQueryable, bool isInnerJoin = false) where T : class, IBaseModel, IEmployeeId, IApprove
        {
            if (isInnerJoin == false)
                return userQueryable.Join(_db.Set<T>(), ol => ol.Id, u => u.EmployeeId, (u, ol) => new { u, ol }).Where(x => x.ol.Status != Status.Pending).Select(x => x.u);
            else
                return userQueryable.GroupJoin(_db.Set<T>(), ol => ol.Id, u => u.EmployeeId, (u, ols) => new { u, ols })
                    .SelectMany(x => x.ols.DefaultIfEmpty(), (u, ol) => new { u = u.u, ol = ol })
                    .Where(x => x.ol != null)
                    .Select(x => x.u);
        }


    }
}
