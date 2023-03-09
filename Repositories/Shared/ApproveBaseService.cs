﻿using System;
using System.Reflection;
using AutoMapper;
using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Enums;
using Helpers.Models.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Common.Interfaces;
using Models.OverrideLogs;
using Models.TimeOnTools;
using Models.WeldingRodRecord;
using Newtonsoft.Json;
using Repositories.Common;
using Repositories.Shared.Interfaces;
using Repositories.Shared.NotificationServices;
using Repositories.Shared.UserInfoServices;
using ViewModels.Notification;
using ViewModels.Shared;

namespace Repositories.Shared
{

    public class ApproveBaseService<TEntity, CreateViewModel, UpdateViewModel, DetailViewModel> : BaseService<TEntity, CreateViewModel, UpdateViewModel, DetailViewModel>, IBaseApprove
        where TEntity : BaseDBModel, IApprove, IEmployeeId, IApproverId
        where DetailViewModel : class, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryResponse _response;
        private readonly IUserInfoService _userInfoService;
        private readonly INotificationService _notificationService;

        public ApproveBaseService(ToranceContext db, ILogger logger, IMapper mapper, IRepositoryResponse response, IUserInfoService userInfoService, INotificationService notificationService) : base(db, logger, mapper, response)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _response = response;
            _userInfoService = userInfoService;
            _notificationService = notificationService;
        }

        public async Task<List<long>> GetApprovedRecordIds()
        {
            try
            {
                var recordIds = await _db.Set<TEntity>().Where(x => x.Status == Enums.Status.Approved).Select(x => x.Id).ToListAsync();
                if (recordIds != null && recordIds.Count > 0)
                {
                    return recordIds;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetApprovedRecordIds method for {typeof(TEntity).FullName} threw an exception.");
            }
            return new List<long>();
        }

        public async Task ApproveRecords(List<long> ids, bool Status)
        {
            try
            {
                var timeSheets = await _db.Set<TEntity>().Where(x => ids.Contains(x.Id)).ToListAsync();
                if (timeSheets != null && timeSheets.Count > 0)
                {
                    if (Status)
                    {
                        timeSheets.ForEach(x => x.Status = Enums.Status.Approved);
                    }
                    else
                    {
                        timeSheets.ForEach(x => x.Status = Enums.Status.Rejected);
                    }
                    await _db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ApproveRecords method for {typeof(TEntity).FullName} threw an exception.");
            }
        }

        public async Task<IRepositoryResponse> SetApproveStatus(long id, Status status)
        {
            using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    var logRecord = await _db.Set<TEntity>().Where(x => x.Id == id).FirstOrDefaultAsync();
                    if (logRecord != null)
                    {
                        logRecord.Status = status;
                        if (logRecord.ApproverId == null)
                        {
                            logRecord.ApproverId = long.Parse(_userInfoService.LoggedInUserId());
                        }
                        await _db.SaveChangesAsync();
                        string type = "";
                        string identifier = "";
                        string identifierKey = "";
                        if (typeof(TEntity).IsAssignableFrom(typeof(TOTLog)))
                        {
                            type = "TOT";
                            identifierKey = "Twr";
                            identifier = (logRecord as TOTLog).Twr;
                        }
                        else if (typeof(TEntity).IsAssignableFrom(typeof(OverrideLog)))
                        {
                            type = "Override";
                            identifierKey = "PO";
                            identifier = (logRecord as OverrideLog).PoNumber.ToString();
                        }
                        else
                        {
                            type = "WRR";
                            identifierKey = "Twr";
                            identifier = (logRecord as WRRLog).Twr.ToString();
                        }
                        var eventType = (status == Status.Approved ? NotificationEventTypeCatalog.Approved : NotificationEventTypeCatalog.Rejected);
                        string notificationTitle = $"{type} Log {status}";
                        string notificationMessage = $"The {type} Log with {identifierKey}# ({identifier}) has been {status}";
                        var userId = await _db.Users.Where(x => x.Id == logRecord.EmployeeId).Select(x => x.Id).FirstOrDefaultAsync();
                        await _notificationService.Create(new NotificationModifyViewModel(logRecord.Id, typeof(TEntity), userId.ToString() ?? "", notificationTitle, notificationMessage, NotificationType.Push, eventType));
                        await transaction.CommitAsync();
                        return _response;
                    }
                    _logger.LogWarning($"No record found for id:{id} for {typeof(TEntity).FullName} in Delete()");

                    await transaction.RollbackAsync();
                    return Response.NotFoundResponse(_response);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, $"ApproveRecords method for {typeof(TEntity).FullName} threw an exception.");
                    return Response.BadRequestResponse(_response);
                }
            }
        }
    }
}

