﻿using AutoMapper;
using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Common.Interfaces;
using Models.WeldingRodRecord;
using Pagination;
using Repositories.Shared;
using Repositories.Shared.UserInfoServices;
using System.Linq.Expressions;
using ViewModels.Shared;
using ViewModels.WeldingRodRecord.WRRLog;

namespace Repositories.Services.WeldRodRecordServices.WRRLogService
{
    public class WRRLogService<CreateViewModel, UpdateViewModel, DetailViewModel> : ApproveBaseService<WRRLog, CreateViewModel, UpdateViewModel, DetailViewModel>, IWRRLogService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger<WRRLogService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryResponse _response;
        private readonly IUserInfoService _userInfoService;

        public WRRLogService(ToranceContext db, ILogger<WRRLogService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IMapper mapper, IRepositoryResponse response, IUserInfoService userInfoService) : base(db, logger, mapper, response)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _response = response;
            _userInfoService = userInfoService;
        }

        public override Expression<Func<WRRLog, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as WRRLogSearchViewModel;
            var loggedInUserId = _userInfoService.LoggedInUserId();
            var loggedInUserRole = _userInfoService.LoggedInUserRole() ?? _userInfoService.LoggedInWebUserRole();
            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.Email.ToLower().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (string.IsNullOrEmpty(searchFilters.Email) || x.Email.ToLower().Contains(searchFilters.Email.ToLower()))
                            &&
                            (searchFilters.Employee.Id == 0 || x.Employee.Id == searchFilters.Employee.Id)
                            &&
                            (searchFilters.Department.Id == 0 || x.Department.Id == searchFilters.Department.Id)
                            &&
                            (searchFilters.Unit.Id == 0 || x.Unit.Id == searchFilters.Unit.Id)
                            //&&
                            //(loggedInUserRole == "SuperAdmin" || x.CreatedBy == loggedInUserId)
                            &&
                            (searchFilters.Location.Id == 0 || x.Location.Id == searchFilters.Location.Id)
            ;
        }

        public override async Task<IRepositoryResponse> GetById(long id)
        {
            try
            {
                var dbModel = await _db.WRRLogs
                    .Include(x => x.Unit)
                    .Include(x => x.Department)
                    .Include(x => x.Location)
                    .Include(x => x.Employee)
                    .Include(x => x.RodType)
                    .Include(x => x.WeldMethod)
                    .Include(x => x.Contractor)
                    .Where(x => x.Id == id).FirstOrDefaultAsync();
                if (dbModel != null)
                {
                    var mappedModel = _mapper.Map<WRRLogDetailViewModel>(dbModel);
                    var response = new RepositoryResponseWithModel<WRRLogDetailViewModel> { ReturnModel = mappedModel };
                    return response;
                }
                _logger.LogWarning($"No record found for id:{id} for WRRLog");
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetById() for WRRLog threw the following exception");
                return Response.BadRequestResponse(_response);
            }
        }

        public async Task<bool> IsWRRLogEmailUnique(int id, string email)
        {
            var check = await _db.WRRLogs.Where(x => x.Email == email && x.Id != id).CountAsync();
            return check < 1;
        }
    }
}
