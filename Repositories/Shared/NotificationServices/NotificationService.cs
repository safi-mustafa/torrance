using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using Enums;
using Helpers.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Models;
using Models.Common;
using Models.Common.Interfaces;
using Models.OverrideLogs;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NotificationService(ToranceContext db, ILogger<NotificationService> logger, IUserInfoService userInfoService, IMapper mapper, IRepositoryResponse response, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _logger = logger;
            _userInfoService = userInfoService;
            _mapper = mapper;
            _response = response;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<IRepositoryResponse> CreateNotificationsForLogCreation(INotificationMetaViewModel meta)
        {
            try
            {
                var model = GetNotificationModel(meta, NotificationEventTypeCatalog.Created);
                bool sendNotifications = true;
                var requestor = await _db.Users.Where(x => x.Id == meta.RequestorId).FirstOrDefaultAsync();
                if (requestor != null && requestor.DisableNotifications == true)
                {
                    sendNotifications = false;
                }
                if (sendNotifications)
                {
                    await SetUserName(model);
                    await SetUnitAndDepartment(model);
                    await SetRequestorName(model);
                    var admins = await GetAdminsForNotifcations();
                    //var association = await GetLogUnitAndDepartment(model);
                    //var approvers = await _db.ApproverAssociations
                    //    .Include(x => x.Approver)
                    //    .Include(x => x.Department)
                    //    .Include(x => x.Unit).
                    //    Where(x => x.UnitId == association.Unit.Id && x.DepartmentId == association.Department.Id && x.Approver.ActiveStatus == ActiveStatus.Active).Distinct().ToListAsync();
                    if (admins != null && admins.Count > 0)
                    {
                        List<Notification> notifications = new List<Notification>();
                        foreach (var admin in admins)
                        {
                            //SetLogPushNotification(model, notifications, admin);
                            SendLogEmailNotification(model, notifications, admin);
                        }
                        _db.Set<Notification>().AddRange(notifications);
                        await _db.SaveChangesAsync();
                    }
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

        public async Task<IRepositoryResponse> CreateNotificationsForLogUpdation(INotificationMetaViewModel meta)
        {
            try
            {
                var model = GetNotificationModel(meta, NotificationEventTypeCatalog.Updated);
                bool sendNotifications = true;
                var requestor = await _db.Users.Where(x => x.Id == meta.RequestorId).FirstOrDefaultAsync();
                if (requestor != null && requestor.DisableNotifications == true)
                {
                    sendNotifications = false;
                }
                if (sendNotifications)
                {
                    await SetUserName(model);
                    await SetApproverName(model);
                    await SetUnitAndDepartment(model);
                    var sendToUsers = await GetSendToUsersForUpdateLog(model);
                    if (sendToUsers != null && sendToUsers.Count > 0)
                    {
                        List<Notification> notifications = new List<Notification>();
                        foreach (var sendToUser in sendToUsers)
                        {
                            if (sendToUser.SendPushNotification)
                            {
                                SetLogPushNotification(model, notifications, sendToUser);
                            }
                            SendLogEmailNotification(model, notifications, sendToUser);
                        }
                        _db.Set<Notification>().AddRange(notifications);
                        await _db.SaveChangesAsync();
                    }
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

        public async Task<IRepositoryResponse> CreateNotificationsForLogApproverAssignment(INotificationMetaViewModel meta)
        {
            try
            {
                var model = GetNotificationModel(meta, NotificationEventTypeCatalog.ApproverAssigned);
                bool sendNotifications = true;
                var requestor = await _db.Users.Where(x => x.Id == meta.RequestorId).FirstOrDefaultAsync();
                if (requestor != null && requestor.DisableNotifications == true)
                {
                    sendNotifications = false;
                }
                if (sendNotifications)
                {
                    await SetUserName(model);
                    await SetApproverName(model);
                    await SetRequestorName(model);
                    await SetUnitAndDepartment(model);

                    var sendToUsers = await GetSendToUsersForApproverAssignmentOfLog(model);
                    if (sendToUsers != null && sendToUsers.Count > 0)
                    {
                        List<Notification> notifications = new List<Notification>();
                        foreach (var sendToUser in sendToUsers)
                        {
                            SetLogPushNotification(model, notifications, sendToUser);
                            SendLogEmailNotification(model, notifications, sendToUser);
                        }
                        _db.Set<Notification>().AddRange(notifications);
                        await _db.SaveChangesAsync();
                    }
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

        public async Task<IRepositoryResponse> CreateNotificationsForLogAfterProcessing(NotificationViewModel model)
        {
            try
            {
                //var model = GetNotificationModel(meta, eventType);
                bool sendNotifications = true;
                var requestor = await _db.Users.Where(x => x.Id == model.RequestorId).FirstOrDefaultAsync();
                if (requestor != null && requestor.DisableNotifications == true)
                {
                    sendNotifications = false;
                }
                if (sendNotifications)
                {
                    await SetUserName(model);
                    await SetApproverName(model);
                    await SetRequestorName(model);
                    await SetUnitAndDepartment(model);


                    var sendToUsers = await GetSendToUsersForProcessedLog(model);
                    if (sendToUsers != null && sendToUsers.Count > 0)
                    {
                        List<Notification> notifications = new List<Notification>();
                        foreach (var sendToUser in sendToUsers)
                        {
                            if (sendToUser.SendPushNotification)
                            {
                                SetLogPushNotification(model, notifications, sendToUser);
                            }
                            SendLogEmailNotification(model, notifications, sendToUser);
                        }
                        _db.Set<Notification>().AddRange(notifications);
                        await _db.SaveChangesAsync();
                    }
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

        //public async Task<IRepositoryResponse> CreateProcessedLogNotification(NotificationViewModel model, long approverId)
        //{
        //    try
        //    {
        //        var association = await GetLogUnitAndDepartment(model);
        //        var approver = await _db.ApproverAssociations.Include(x => x.Approver).Include(x => x.Department).Include(x => x.Unit)
        //            .Where(x => x.UnitId == association.Unit.Id && x.DepartmentId == association.Department.Id && x.Approver.ActiveStatus == ActiveStatus.Active && x.ApproverId == approverId).AsNoTracking().FirstOrDefaultAsync();
        //        if (approver == null)
        //        {
        //            approver = new ApproverAssociation();
        //            approver.Approver = await _db.Users.Where(x => x.Id == approverId).FirstOrDefaultAsync();
        //            approver.Unit = new Unit { Id = association.Unit?.Id ?? 0, Name = association.Unit?.Name };
        //            approver.Department = new Department { Id = association.Department?.Id ?? 0, Name = association.Department?.Name };
        //        }
        //        List<Notification> notifications = new List<Notification>();
        //        //SetProcessedLogPushNotification(model, notifications);
        //        await SendProcessedLogEmailNotification(model, notifications, approver);
        //        _db.Set<Notification>().AddRange(notifications);
        //        await _db.SaveChangesAsync();
        //        var response = new RepositoryResponseWithModel<long> { ReturnModel = 1 };
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Exception thrown in CreateLogNotification method of {typeof(Notification).FullName}");
        //        return Response.BadRequestResponse(_response);
        //    }
        //}

        public async Task<IRepositoryResponse> Create(NotificationViewModel model)
        {
            try
            {
                var mappedModel = _mapper.Map<Notification>(model);
                //if (string.IsNullOrEmpty(mappedModel.Message))
                //    mappedModel.Message = JsonConvert.SerializeObject(new LogPushNotificationViewModel(model));
                mappedModel.Id = Guid.NewGuid();
                mappedModel.CreatedOn = DateTime.UtcNow;
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



        private void SendLogEmailNotification(NotificationViewModel model, List<Notification> notifications, NotificationSendToModel sendTo)
        {

            var notificationMappedModel = _mapper.Map<Notification>(model);
            notificationMappedModel.Id = Guid.NewGuid();

            notificationMappedModel.Message = JsonConvert.SerializeObject(new LogEmailViewModel(model, sendTo, notificationMappedModel.Id, _configuration));
            notificationMappedModel.SendTo = sendTo.Id;
            notificationMappedModel.Type = NotificationType.Email;
            notificationMappedModel.CreatedOn = DateTime.UtcNow;
            notificationMappedModel.IsSent = false;
            notifications.Add(notificationMappedModel);
        }


        private void SetLogPushNotification(NotificationViewModel model, List<Notification> notifications, NotificationSendToModel sendTo)
        {
            var notificationMappedModel = _mapper.Map<Notification>(model);
            notificationMappedModel.Message = JsonConvert.SerializeObject(new LogPushNotificationViewModel(model, sendTo));
            notificationMappedModel.Id = Guid.NewGuid();
            notificationMappedModel.SendTo = sendTo.Id;
            notificationMappedModel.Type = NotificationType.Push;
            notificationMappedModel.CreatedOn = DateTime.UtcNow;
            notifications.Add(notificationMappedModel);
        }

        //private void SetProcessedLogPushNotification(NotificationViewModel model, List<Notification> notifications)
        //{
        //    var notificationMappedModel = _mapper.Map<Notification>(model);
        //    notificationMappedModel.Message = JsonConvert.SerializeObject(new LogPushNotificationViewModel(model));
        //    notificationMappedModel.Id = Guid.NewGuid();
        //    notificationMappedModel.SendTo = model.SendTo;
        //    notificationMappedModel.Type = NotificationType.Push;
        //    notificationMappedModel.CreatedOn = DateTime.UtcNow;
        //    //notificationMappedModel.SendTo = model.Type == NotificationType.Email ? approver.Approver?.Email ?? "" : approver.ApproverId.ToString();
        //    notifications.Add(notificationMappedModel);
        //}

        //private async Task<ApproverAssociationsViewModel> GetLogUnitAndDepartment(NotificationViewModel viewModel)
        //{
        //    ApproverAssociationsViewModel association = new ApproverAssociationsViewModel();
        //    if (viewModel.EntityType == NotificationEntityType.TOTLog)
        //    {
        //        association = await _db.TOTLogs.Include(x => x.Department).Include(x => x.Unit).Where(x => x.Id == viewModel.EntityId).Select(x =>
        //        new ApproverAssociationsViewModel
        //        {
        //            Department = new DepartmentBriefViewModel
        //            {
        //                Id = x.DepartmentId,
        //                Name = x.Department.Name
        //            },
        //            Unit = new UnitBriefViewModel()
        //            {
        //                Id = x.UnitId,
        //                Name = x.Unit.Name
        //            },
        //        }).FirstOrDefaultAsync();
        //    }
        //    else if (viewModel.EntityType == NotificationEntityType.WRRLog)
        //    {
        //        association = await _db.WRRLogs.Include(x => x.Department).Include(x => x.Unit).Where(x => x.Id == viewModel.EntityId).Select(x =>
        //        new ApproverAssociationsViewModel
        //        {
        //            Department = new DepartmentBriefViewModel
        //            {
        //                Id = x.DepartmentId,
        //                Name = x.Department.Name
        //            },
        //            Unit = new UnitBriefViewModel()
        //            {
        //                Id = x.UnitId,
        //                Name = x.Unit.Name
        //            },
        //        }).FirstOrDefaultAsync();
        //    }
        //    else
        //    {
        //        association = await _db.OverrideLogs.Include(x => x.Department).Include(x => x.Unit).Where(x => x.Id == viewModel.EntityId).Select(x =>
        //        new ApproverAssociationsViewModel
        //        {
        //            Department = new DepartmentBriefViewModel
        //            {
        //                Id = x.DepartmentId,
        //                Name = x.Department.Name
        //            },
        //            Unit = new UnitBriefViewModel()
        //            {
        //                Id = x.UnitId,
        //                Name = x.Unit.Name
        //            },
        //        }).FirstOrDefaultAsync();
        //    }
        //    return association;
        //}

        private async Task SetUnitAndDepartment(NotificationViewModel model)
        {
            var departmentId = long.Parse(model.DepartmentId);
            var unitId = long.Parse(model.UnitId);
            model.Department = await _db.Departments.Where(x => x.Id == departmentId).Select(x => x.Name).FirstOrDefaultAsync();
            model.Unit = await _db.Units.Where(x => x.Id == unitId).Select(x => x.Name).FirstOrDefaultAsync();
        }

        private async Task<List<NotificationSendToModel>> GetAdminsForNotifcations()
        {
            var targetRoles = new List<string>() { RolesCatalog.Administrator.ToString() };
            var adminUserQueryable = (from r in _db.Roles
                                      join ur in _db.UserRoles on r.Id equals ur.RoleId
                                      join u in _db.Users on ur.UserId equals u.Id
                                      where targetRoles.Contains(r.Name)
                                      && u.IsDeleted == false
                                      && u.ActiveStatus == ActiveStatus.Active
                                      && u.DisableNotifications == false
                                      select new NotificationSendToModel
                                      {
                                          Id = ur.UserId.ToString(),
                                          Name = u.FullName,
                                          SendPushNotification = false

                                      }).AsQueryable();

            var adminUsers = await adminUserQueryable.ToListAsync();
            return adminUsers;
        }
        private async Task<List<NotificationSendToModel>> GetSendToUsersForUpdateLog(NotificationViewModel model)
        {
            if (string.IsNullOrEmpty(model.ApproverId))
            {
                return await GetAdminsForNotifcations();
            }
            else
            {
                var approverId = long.Parse(model.ApproverId);
                return await (_db.Users.Where(x => x.Id == approverId).Select(u => new NotificationSendToModel
                {
                    Id = u.Id.ToString(),
                    Name = u.FullName
                }).ToListAsync());
            }

        }
        private async Task<List<NotificationSendToModel>> GetSendToUsersForApproverAssignmentOfLog(NotificationViewModel model)
        {
            List<NotificationSendToModel> sendToUsers = new List<NotificationSendToModel>();
            sendToUsers.Add(new NotificationSendToModel()
            {
                Id = model.RequestorId?.ToString(),
                Name = model.Requestor
            });
            sendToUsers.Add(new NotificationSendToModel()
            {
                Id = model.ApproverId,
                Name = model.Approver
            });
            return sendToUsers;

        }
        private async Task<List<NotificationSendToModel>> GetSendToUsersForProcessedLog(NotificationViewModel model)
        {
            var sendToUsers = await GetAdminsForNotifcations();
            var requestorId = model.RequestorId;
            var requestor = await _db.Users.Where(x => x.Id == requestorId).Select(u => new NotificationSendToModel
            {
                Id = u.Id.ToString(),
                Name = u.FullName
            }).FirstOrDefaultAsync();
            sendToUsers.Add(requestor);
            return sendToUsers;


        }
        private async Task SetUserName(NotificationViewModel model)
        {
            model.User = "";
            string userId = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                model.User = await _db.Users.Where(x => x.Id == long.Parse(userId)).Select(x => x.FullName).FirstOrDefaultAsync();
            }
        }

        private async Task SetRequestorName(NotificationViewModel model)
        {
            if (model.RequestorId > 0)
            {
                model.Requestor = await _db.Users.Where(x => x.Id == model.RequestorId).Select(x => x.FullName).FirstOrDefaultAsync();
            }
        }

        private async Task SetApproverName(NotificationViewModel model)
        {
            model.Approver = "";
            string userId = model.ApproverId;
            if (!string.IsNullOrEmpty(userId))
            {
                model.Approver = await _db.Users.Where(x => x.Id == long.Parse(userId)).Select(x => x.FullName).FirstOrDefaultAsync();
            }
        }

        private NotificationViewModel GetNotificationModel(INotificationMetaViewModel model, NotificationEventTypeCatalog eventType)
        {
            return new NotificationViewModel()
            {
                Id = Guid.NewGuid(),
                LogId = model.LogId,
                EntityId = model.LogId,
                EventType = eventType,
                EntityType = model.EntityType,
                IdentifierKey = model.IdentifierKey,
                IdentifierValue = model.IdentifierValue,
                RequestorId = model.RequestorId,
                DepartmentId = model.DepartmentId,
                UnitId = model.UnitId,
                ApproverId = model.ApproverId,
            };
        }
    }
}
