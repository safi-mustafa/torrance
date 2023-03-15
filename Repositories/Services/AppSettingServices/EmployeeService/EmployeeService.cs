using AutoMapper;
using DataLibrary;
using Microsoft.Extensions.Logging;
using Repositories.Shared.AuthenticationService;
using Centangle.Common.ResponseHelpers.Models;
using Models.Common.Interfaces;
using ViewModels.Shared;
using Microsoft.AspNetCore.Identity;
using Models;
using Repositories.Services.CommonServices.UserService;
using ExcelReader.Repository;
using ViewModels.WeldingRodRecord.Employee;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Reflection;
using System.Data;
using System.ComponentModel;
using ViewModels.Common.Company;

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
        private readonly IExcelReader _excelReader;

        public EmployeeService(ToranceContext db, UserManager<ToranceUser> userManager, ILogger<EmployeeService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IMapper mapper, IIdentityService identity, IRepositoryResponse response, IExcelReader excelReader)
            :
            base(db, Enums.RolesCatalog.Employee, userManager, logger, mapper, identity, response)
        {
            _db = db;
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
            _identity = identity;
            _response = response;
            _excelReader = excelReader;
        }

        public async Task<bool> InitializeExcelContractData(ExcelFileVM model)
        {
            var data = _excelReader.GetData(model.File.OpenReadStream());
            //var excelDataList = AddModel<EmployeeModifyViewModel>(data, 0, 0);
            var excelDataList = _excelReader.AddModel<EmployeeExcelViewModel>(data, 0, 0);
            var employeeList = _mapper.Map<List<EmployeeModifyViewModel>>(excelDataList);
            var response = await ProcessExcelData(employeeList);
            return response;
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

    }
}
