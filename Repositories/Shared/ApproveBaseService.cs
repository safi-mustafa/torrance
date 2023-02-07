﻿using System;
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
using Newtonsoft.Json;
using Repositories.Common;
using Repositories.Shared.Interfaces;
using Repositories.Shared.NotificationServices;
using ViewModels.Notification;
using ViewModels.Shared;

namespace Repositories.Shared
{

    public class ApproveBaseService<TEntity, CreateViewModel, UpdateViewModel, DetailViewModel> : BaseService<TEntity, CreateViewModel, UpdateViewModel, DetailViewModel>, IBaseApprove
        where TEntity : BaseDBModel, IApprove, IEmployeeId
        where DetailViewModel : class, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryResponse _response;
        private readonly INotificationService _notificationService;

        public ApproveBaseService(ToranceContext db, ILogger logger, IMapper mapper, IRepositoryResponse response, INotificationService notificationService) : base(db, logger, mapper, response)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _response = response;
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
                        await _db.SaveChangesAsync();
                        await _notificationService.AddNotificationAsync(new NotificationViewModel(logRecord.Id, typeof(TEntity), logRecord.EmployeeId?.ToString() ?? "", "Log Status Updated", $"Log for {logRecord.CreatedOn.ToString("U")} has been {status}", NotificationType.Push, NotificationEntityType.Logs));
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

