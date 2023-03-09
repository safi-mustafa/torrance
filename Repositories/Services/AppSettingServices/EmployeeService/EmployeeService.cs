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
            var excelDataList = _excelReader.AddModel<EmployeeModifyViewModel>(data, 0, 0);
            var response = await ProcessExcelData(excelDataList);
            return response;
        }

        private async Task<bool> ProcessExcelData(List<EmployeeModifyViewModel> excelDataList)
        {
            try
            {
                var existingEmployees = await _db.Employees.Select(x => new { Id = x.Id, Email = x.Email }).ToListAsync();
                foreach (var item in excelDataList)
                {
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
