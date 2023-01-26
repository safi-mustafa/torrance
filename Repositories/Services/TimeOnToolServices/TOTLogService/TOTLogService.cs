using AutoMapper;
using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Enums;
using Helpers.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Common.Interfaces;
using Models.TimeOnTools;
using Pagination;
using Repositories.Common;
using Repositories.Shared;
using Repositories.Shared.UserInfoServices;
using System.Linq.Expressions;
using ViewModels.Shared;
using ViewModels.TomeOnTools.TOTLog;

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
            var loggedInUserRole = _userInfoService.LoggedInUserRole() ?? _userInfoService.LoggedInWebUserRole();
            var loggedInUserId = loggedInUserRole == "Employee" ? _userInfoService.LoggedInEmployeeId() : _userInfoService.LoggedInUserId();
            var parsedLoggedInId = long.Parse(loggedInUserId);

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.EquipmentNo.ToString().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (searchFilters.EquipmentNo == 0 || x.EquipmentNo == searchFilters.EquipmentNo)
                            &&
                            (searchFilters.Contractor.Id == 0 || x.Contractor.Id == searchFilters.Contractor.Id)
                            &&
                            (searchFilters.Department.Id == 0 || x.Department.Id == searchFilters.Department.Id)
                            &&
                            (searchFilters.Unit.Id == 0 || x.Unit.Id == searchFilters.Unit.Id)
                            &&
                            (
                                (loggedInUserRole == "SuperAdmin")
                                ||
                                (loggedInUserRole == "Approver" && x.ApproverId == parsedLoggedInId)
                                ||
                                (loggedInUserRole == "Employee" && x.EmployeeId == parsedLoggedInId)
                            )
                            &&
                            (searchFilters.Status == null || searchFilters.Status == x.Status)
            ;
        }

        //public override async Task<IRepositoryResponse> GetAll<M>(IBaseSearchModel search)
        //{
        //    var searchFilters = search as TOTLogSearchViewModel;
        //    var loggedInUserRole = _userInfoService.LoggedInUserRole() ?? _userInfoService.LoggedInWebUserRole();
        //    var loggedInUserId = loggedInUserRole == "Employee" ? _userInfoService.LoggedInEmployeeId() : _userInfoService.LoggedInUserId();
        //    var parsedLoggedInId = long.Parse(loggedInUserId);

        //    try
        //    {
        //        var filters = SetQueryFilter(search);
        //        var result =  _db.TOTLogs.Where(filters).AsQueryable();

        //        if(loggedInUserRole == "Employee")
        //        {
        //            result = result.Where(x => x.CreatedBy == parsedLoggedInId).AsQueryable();
        //        }
        //        if(loggedInUserRole == "Approver")
        //        {
        //            result = result.Where(x => x.ApproverId == parsedLoggedInId).AsQueryable();
        //        }
        //        var paginatedResult = await result.Paginate(search);
        //        if (paginatedResult != null)
        //        {
        //            var paginatedResponse = new PaginatedResultModel<M>();
        //            paginatedResponse.Items = _mapper.Map<List<M>>(paginatedResult.Items.ToList());
        //            paginatedResponse._meta = paginatedResult._meta;
        //            paginatedResponse._links = paginatedResult._links;
        //            var response = new RepositoryResponseWithModel<PaginatedResultModel<M>> { ReturnModel = paginatedResponse };
        //            return response;
        //        }
        //        _logger.LogWarning($"No record found for TOTLog in GetAll()");
        //        return Response.NotFoundResponse(_response);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"GetAll() method for TOTLogs threw an exception.");
        //        return Response.BadRequestResponse(_response);
        //    }

        //   // return base.GetAll<M>(search);
        //}

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
                    .Include(x => x.PermittingIssue)
                    .Where(x => x.Id == id).FirstOrDefaultAsync();
                if (dbModel != null)
                {
                    var mappedModel = _mapper.Map<TOTLogDetailViewModel>(dbModel);
                    var response = new RepositoryResponseWithModel<TOTLogDetailViewModel> { ReturnModel = mappedModel };
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

    }
}
