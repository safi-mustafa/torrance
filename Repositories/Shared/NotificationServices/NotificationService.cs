using System.Linq.Expressions;
using AutoMapper;
using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Enums;
using Helpers.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Models;
using Models.Common;
using Models.Common.Interfaces;
using Newtonsoft.Json;
using Pagination;
using Repositories.Common;
using Repositories.Interfaces;
using Repositories.Shared.UserInfoServices;
using ViewModels.Authentication.Approver;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;
using ViewModels.Notification;
using ViewModels.Shared;

namespace Repositories.Shared.NotificationServices
{
    public class NotificationService : INotificationService
    {
        private readonly ToranceContext _db;
        private readonly ILogger<NotificationService> _logger;
        private readonly IUserInfoService _userInfoService;
        private readonly IMapper _mapper;
        private readonly IRepositoryResponse _response;
        private readonly IConfiguration _configuration;

        public NotificationService(ToranceContext db, ILogger<NotificationService> logger, IUserInfoService userInfoService, IMapper mapper, IRepositoryResponse response, IConfiguration configuration)
        {
            _db = db;
            _logger = logger;
            _userInfoService = userInfoService;
            _mapper = mapper;
            _response = response;
            _configuration = configuration;
        }

        public async Task<IRepositoryResponse> CreateLogNotification(NotificationViewModel model)
        {
            try
            {
                var association = await GetLogUnitAndDepartment(model);
                var approvers = await _db.ApproverAssociations
                    .Include(x => x.Approver)
                    .Include(x => x.Department)
                    .Include(x => x.Unit).
                    Where(x => x.UnitId == association.Unit.Id && x.DepartmentId == association.Department.Id && x.Approver.ActiveStatus == ActiveStatus.Active).Distinct().ToListAsync();
                if (approvers != null && approvers.Count > 0)
                {
                    List<Notification> notifications = new List<Notification>();
                    foreach (var approver in approvers)
                    {
                        SetLogPushNotification(model, notifications, approver);
                        SendLogEmailNotification(model, notifications, approver);
                    }
                    _db.Set<Notification>().AddRange(notifications);
                    await _db.SaveChangesAsync();
                }
                var response = new RepositoryResponseWithModel<long> { ReturnModel = 1 };
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown in CreateLogNotification method of {typeof(Notification).FullName}");
                return Response.BadRequestResponse(_response);
            }
        }

        public async Task<IRepositoryResponse> CreateProcessedLogNotification(NotificationViewModel model, long approverId)
        {
            try
            {
                var association = await GetLogUnitAndDepartment(model);
                var approver = await _db.ApproverAssociations.Include(x => x.Approver).Include(x => x.Department).Include(x => x.Unit)
                    .Where(x => x.UnitId == association.Unit.Id && x.DepartmentId == association.Department.Id && x.ActiveStatus == ActiveStatus.Active && x.ApproverId == approverId).FirstOrDefaultAsync();

                List<Notification> notifications = new List<Notification>();
                //SetProcessedLogPushNotification(model, notifications);
                SendProcessedLogEmailNotification(model, notifications, approver);
                _db.Set<Notification>().AddRange(notifications);
                await _db.SaveChangesAsync();
                var response = new RepositoryResponseWithModel<long> { ReturnModel = 1 };
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown in CreateLogNotification method of {typeof(Notification).FullName}");
                return Response.BadRequestResponse(_response);
            }
        }

        private void SendLogEmailNotification(NotificationViewModel model, List<Notification> notifications, ApproverAssociation? approver)
        {

            var notificationMappedModel = _mapper.Map<Notification>(model);
            notificationMappedModel.Id = Guid.NewGuid();
            var domain = _configuration["WebUrl"];
            var approvalLink = $"{domain}/Approval/ApproveByNotification?id={notificationMappedModel.Id}";
            notificationMappedModel.Message = JsonConvert.SerializeObject(new LogEmailViewModel(model, approver, approvalLink));
            notificationMappedModel.SendTo = approver.ApproverId.ToString();
            notificationMappedModel.Type = NotificationType.Email;
            notificationMappedModel.CreatedOn = DateTime.Now;
            notificationMappedModel.IsSent = false;
            notifications.Add(notificationMappedModel);
        }

        private void SendProcessedLogEmailNotification(NotificationViewModel model, List<Notification> notifications, ApproverAssociation? approver)
        {
            var notificationMappedModel = _mapper.Map<Notification>(model);
            notificationMappedModel.Id = Guid.NewGuid();

            notificationMappedModel.Message = JsonConvert.SerializeObject(new LogEmailViewModel(model, approver, SentEmailType.LogProcessed));
            notificationMappedModel.SendTo = model.SendTo;
            notificationMappedModel.Type = NotificationType.Email;
            notificationMappedModel.CreatedOn = DateTime.Now;
            notificationMappedModel.IsSent = false;
            notifications.Add(notificationMappedModel);
        }

        private void SetLogPushNotification(NotificationViewModel model, List<Notification> notifications, ApproverAssociation? approver)
        {
            var notificationMappedModel = _mapper.Map<Notification>(model);
            notificationMappedModel.Message = JsonConvert.SerializeObject(new LogPushNotificationViewModel(model));
            notificationMappedModel.Id = Guid.NewGuid();
            notificationMappedModel.SendTo = approver.ApproverId.ToString();
            notificationMappedModel.Type = NotificationType.Push;
            notificationMappedModel.CreatedOn = DateTime.Now;
            //notificationMappedModel.SendTo = model.Type == NotificationType.Email ? approver.Approver?.Email ?? "" : approver.ApproverId.ToString();
            notifications.Add(notificationMappedModel);
        }

        private void SetProcessedLogPushNotification(NotificationViewModel model, List<Notification> notifications)
        {
            var notificationMappedModel = _mapper.Map<Notification>(model);
            notificationMappedModel.Message = JsonConvert.SerializeObject(new LogPushNotificationViewModel(model));
            notificationMappedModel.Id = Guid.NewGuid();
            notificationMappedModel.SendTo = model.SendTo;
            notificationMappedModel.Type = NotificationType.Push;
            notificationMappedModel.CreatedOn = DateTime.Now;
            //notificationMappedModel.SendTo = model.Type == NotificationType.Email ? approver.Approver?.Email ?? "" : approver.ApproverId.ToString();
            notifications.Add(notificationMappedModel);
        }

        public async Task<IRepositoryResponse> Create(NotificationViewModel model)
        {
            try
            {
                var mappedModel = _mapper.Map<Notification>(model);
                if (string.IsNullOrEmpty(mappedModel.Message))
                    mappedModel.Message = JsonConvert.SerializeObject(new LogPushNotificationViewModel(model));
                mappedModel.Id = Guid.NewGuid();
                mappedModel.CreatedOn = DateTime.Now;
                await _db.Set<Notification>().AddAsync(mappedModel);
                await _db.SaveChangesAsync();

                var response = new RepositoryResponseWithModel<long> { ReturnModel = 1 };
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown in Create method of {typeof(Notification).FullName}");
                return Response.BadRequestResponse(_response);
            }
        }

        public virtual async Task<IRepositoryResponse> GetAll<M>(IBaseSearchModel searchFilter)
        {
            try
            {
                var search = searchFilter as NotificationSearchViewModel;
                var loggedInUserId = _userInfoService.LoggedInUserId();
                var loggedInUserRole = _userInfoService.LoggedInUserRole();
                var result = await _db.Notifications.Where(x =>
                            (string.IsNullOrEmpty(search.Search.value) || x.Type.ToString().ToLower().Contains(search.Search.value.ToLower()))
                            &&
                            (search.Type == null || x.Type == search.Type)
                            &&
                            (search.IsSent == null || x.IsSent == search.IsSent)
                            &&
                            (loggedInUserRole == "SuperAdmin" || x.SendTo == loggedInUserId)
                     ).Paginate(search);
                if (result != null)
                {
                    var paginatedResult = new PaginatedResultModel<M>();
                    paginatedResult.Items = _mapper.Map<List<M>>(result.Items.ToList());
                    paginatedResult._meta = result._meta;
                    paginatedResult._links = result._links;
                    var response = new RepositoryResponseWithModel<PaginatedResultModel<M>> { ReturnModel = paginatedResult };
                    return response;
                }
                _logger.LogWarning($"No record found for {typeof(Notification).FullName} in GetAll()");
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetAll() method for {typeof(Notification).FullName} threw an exception.");
                return Response.BadRequestResponse(_response);
            }
        }

        private async Task<ApproverAssociationsViewModel> GetLogUnitAndDepartment(NotificationViewModel viewModel)
        {
            ApproverAssociationsViewModel association = new ApproverAssociationsViewModel();
            if (viewModel.EntityType == NotificationEntityType.TOTLog)
            {
                association = await _db.TOTLogs.Where(x => x.Id == viewModel.EntityId).Select(x =>
                new ApproverAssociationsViewModel
                {
                    Department = new DepartmentBriefViewModel
                    {
                        Id = x.DepartmentId,
                    },
                    Unit = new UnitBriefViewModel()
                    {
                        Id = x.UnitId,
                    },
                }).FirstOrDefaultAsync();
            }
            else if (viewModel.EntityType == NotificationEntityType.WRRLog)
            {
                association = await _db.WRRLogs.Where(x => x.Id == viewModel.EntityId).Select(x =>
                new ApproverAssociationsViewModel
                {
                    Department = new DepartmentBriefViewModel
                    {
                        Id = x.DepartmentId,
                    },
                    Unit = new UnitBriefViewModel()
                    {
                        Id = x.UnitId,
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
                        Id = x.DepartmentId,
                    },
                    Unit = new UnitBriefViewModel()
                    {
                        Id = x.UnitId,
                    }
                }).FirstOrDefaultAsync();
            }
            return association;
        }
    }
}
