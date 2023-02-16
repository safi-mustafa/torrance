using System.Linq.Expressions;
using AutoMapper;
using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using Models.Common;
using Models.Common.Interfaces;
using Pagination;
using Repositories.Common;
using Repositories.Shared.UserInfoServices;
using ViewModels.Authentication.Approver;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.Notification;
using ViewModels.Shared;

namespace Repositories.Shared.NotificationServices
{
    public class NotificationService<CreateViewModel, UpdateViewModel, DetailViewModel> : BaseService<Notification, CreateViewModel, UpdateViewModel, DetailViewModel>, INotificationService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger<NotificationService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IUserInfoService _userInfoService;
        private readonly IMapper _mapper;
        private readonly IRepositoryResponse _response;

        public NotificationService(ToranceContext db, ILogger<NotificationService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IUserInfoService userInfoService, IMapper mapper, IRepositoryResponse response) : base(db, logger, mapper, response)
        {
            _db = db;
            _logger = logger;
            _userInfoService = userInfoService;
            _mapper = mapper;
            _response = response;
        }

        public override Expression<Func<Notification, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as NotificationSearchViewModel;
            var loggedInUserId = _userInfoService.LoggedInUserId();
            var loggedInUserRole = _userInfoService.LoggedInUserRole();
            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.Type.ToString().ToLower().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (searchFilters.Type == null || x.Type == searchFilters.Type)
                            &&
                            (searchFilters.IsSent == null || x.IsSent == searchFilters.IsSent)
                            &&
                            (loggedInUserRole == "SuperAdmin" || x.SendTo == loggedInUserId)
                        ;
        }
        public override async Task<IRepositoryResponse> Create(CreateViewModel model)
        {
            var viewModel = model as NotificationModifyViewModel;
            try
            {
                if ((viewModel.EventType == NotificationEventTypeCatalog.Created || viewModel.EventType == NotificationEventTypeCatalog.Updated) && (string.IsNullOrEmpty(viewModel.SendTo) || viewModel.SendTo == "0"))
                {
                    var association = await GetLogUnitAndDepartment(viewModel);
                    var approvers = await _db.ApproverAssociations.Where(x => x.UnitId == association.Unit.Id && x.DepartmentId == association.Department.Id).Distinct().ToListAsync();
                    if (approvers != null && approvers.Count > 0)
                    {
                        List<Notification> notifications = new List<Notification>();
                        foreach (var approver in approvers)
                        {
                            var notificationMappedModel = _mapper.Map<Notification>(model);
                            notificationMappedModel.SendTo = approver.ApproverId.ToString();
                            notifications.Add(notificationMappedModel);

                        }
                        _db.Set<Notification>().AddRange(notifications);
                        await _db.SaveChangesAsync();
                    }
                }
                else
                {
                    var mappedModel = _mapper.Map<Notification>(model);
                    await _db.Set<Notification>().AddAsync(mappedModel);
                    await _db.SaveChangesAsync();
                }


                var response = new RepositoryResponseWithModel<long> { ReturnModel = 1 };
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown in Create method of {typeof(Notification).FullName}");
                return Response.BadRequestResponse(_response);
            }
        }

        private async Task<ApproverAssociationsViewModel> GetLogUnitAndDepartment(NotificationModifyViewModel viewModel)
        {
            ApproverAssociationsViewModel association = new ApproverAssociationsViewModel();
            if (viewModel.EntityType == NotificationEntityType.TOTLog)
            {
                association = await _db.TOTLogs.Where(x => x.Id == viewModel.EntityId).Select(x =>
                new ApproverAssociationsViewModel
                {
                    Department = new DepartmentBriefViewModel
                    {
                        Id = x.DepartmentId
                    },
                    Unit = new UnitBriefViewModel()
                    {
                        Id = x.UnitId
                    }
                }).FirstOrDefaultAsync();
            }
            else if (viewModel.EntityType == NotificationEntityType.WRRLog)
            {
                association = await _db.WRRLogs.Where(x => x.Id == viewModel.EntityId).Select(x =>
                new ApproverAssociationsViewModel
                {
                    Department = new DepartmentBriefViewModel
                    {
                        Id = x.DepartmentId
                    },
                    Unit = new UnitBriefViewModel()
                    {
                        Id = x.UnitId
                    }
                }).FirstOrDefaultAsync();
            }
            else
            {
                association = await _db.OverrideLogs.Where(x => x.Id == viewModel.EntityId).Select(x =>
                new ApproverAssociationsViewModel
                {
                    Department = new DepartmentBriefViewModel
                    {
                        Id = x.DepartmentId
                    },
                    Unit = new UnitBriefViewModel()
                    {
                        Id = x.UnitId
                    }
                }).FirstOrDefaultAsync();
            }
            return association;
        }
    }
}
