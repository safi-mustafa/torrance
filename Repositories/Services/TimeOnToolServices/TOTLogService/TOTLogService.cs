using AutoMapper;
using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Enums;
using Helpers.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Common.Interfaces;
using Models.OverrideLogs;
using Models.TimeOnTools;
using Pagination;
using Repositories.Common;
using Repositories.Shared;
using Repositories.Shared.UserInfoServices;
using System.Data;
using System.Linq.Expressions;
using ViewModels.OverrideLogs.ORLog;
using ViewModels.Shared;
using ViewModels.TimeOnTools.TOTLog;

namespace Repositories.Services.TimeOnToolServices.TOTLogService
{
    public class TOTLogService<CreateViewModel, UpdateViewModel, DetailViewModel> : ApproveBaseService<TOTLog, CreateViewModel, UpdateViewModel, DetailViewModel>, ITOTLogService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger<TOTLogService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryResponse _response;
        private readonly IUserInfoService _userInfoService;

        public TOTLogService(ToranceContext db, ILogger<TOTLogService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IMapper mapper, IRepositoryResponse response, IUserInfoService userInfoService) : base(db, logger, mapper, response)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _response = response;
            _userInfoService = userInfoService;
        }

        public override Expression<Func<TOTLog, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as TOTLogSearchViewModel;
            searchFilters.OrderByColumn = "Status";
            var status = (Status?)((int?)searchFilters.Status);
            var loggedInUserRole = _userInfoService.LoggedInUserRole() ?? _userInfoService.LoggedInWebUserRole();
            var loggedInUserId = loggedInUserRole == "Employee" ? _userInfoService.LoggedInEmployeeId() : _userInfoService.LoggedInUserId();
            var parsedLoggedInId = long.Parse(loggedInUserId);

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.EquipmentNo.ToString().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (searchFilters.EquipmentNo == null || x.EquipmentNo == searchFilters.EquipmentNo)
                            //&&
                            //(searchFilters.Contractor.Id == 0 || x.Contractor.Id == searchFilters.Contractor.Id)
                            //&&
                            //(searchFilters.Department.Id == 0 || x.Department.Id == searchFilters.Department.Id)
                            &&
                            (searchFilters.Unit.Id == 0 || searchFilters.Unit.Id == null || x.Unit.Id == searchFilters.Unit.Id)
                            &&
                            (
                                (loggedInUserRole == "SuperAdmin")
                                ||
                                (loggedInUserRole == "Approver" && x.ApproverId == parsedLoggedInId)
                                ||
                                (loggedInUserRole == "Employee" && x.EmployeeId == parsedLoggedInId)
                            )
                            &&
                            (status == null || status == x.Status)
                            &&
                            (searchFilters.StatusNot == null || searchFilters.StatusNot != x.Status)
            ;
        }

        public override async Task<IRepositoryResponse> GetById(long id)
        {
            try
            {
                var dbModel = await _db.TOTLogs
                    .Include(x => x.Unit)
                    .Include(x => x.Department)
                    .Include(x => x.Contractor)
                    .Include(x => x.ReworkDelay)
                    .Include(x => x.ShiftDelay)
                    .Include(x => x.Shift)
                    .Include(x => x.PermitType)
                    .Include(x => x.Approver)
                    .Include(x => x.Foreman)
                    .Include(x => x.Employee)
                    .Include(x => x.PermittingIssue)
                    .Where(x => x.Id == id).FirstOrDefaultAsync();
                if (dbModel != null)
                {
                    var mappedModel = _mapper.Map<DetailViewModel>(dbModel);
                    var response = new RepositoryResponseWithModel<DetailViewModel> { ReturnModel = mappedModel };
                    return response;
                }
                _logger.LogWarning($"No record found for id:{id} for TOTLog");
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetById() for TOTLog threw the following exception");
                return Response.BadRequestResponse(_response);
            }
        }

        public async override Task<IRepositoryResponse> Create(CreateViewModel model)
        {
            try
            {
                var mappedModel = _mapper.Map<TOTLog>(model);
                await SetRequesterId(mappedModel);
                await _db.Set<TOTLog>().AddAsync(mappedModel);
                await _db.SaveChangesAsync();
                var response = new RepositoryResponseWithModel<long> { ReturnModel = mappedModel.Id };
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown in Create method of {typeof(TOTLog).FullName}");
                return Response.BadRequestResponse(_response);
            }
        }

        public async override Task<IRepositoryResponse> Update(UpdateViewModel model)
        {
            try
            {
                var updateModel = model as BaseUpdateVM;
                if (updateModel != null)
                {
                    var record = await _db.Set<TOTLog>().FindAsync(updateModel?.Id);
                    if (record != null)
                    {
                        var dbModel = _mapper.Map(model, record);
                        await SetRequesterId(dbModel);
                        await _db.SaveChangesAsync();
                        var response = new RepositoryResponseWithModel<long> { ReturnModel = record.Id };
                        return response;
                    }
                    _logger.LogWarning($"Record for id: {updateModel?.Id} not found in {typeof(TOTLog).FullName} in Update()");
                }
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Update() for {typeof(TOTLog).FullName} threw the following exception");
                return Response.BadRequestResponse(_response);
            }
        }

        private async Task SetRequesterId(TOTLog mappedModel)
        {
            var role = _userInfoService.LoggedInUserRole();
            if (role != "Employee")
            {
                mappedModel.EmployeeId = long.Parse(_userInfoService.LoggedInUserId());
            }
            mappedModel.CompanyId = await _db.Employees.Where(x => x.Id == mappedModel.EmployeeId).Select(x => x.CompanyId).FirstOrDefaultAsync();
        }
    }
}
