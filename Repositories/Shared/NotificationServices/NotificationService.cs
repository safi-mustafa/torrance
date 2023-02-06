using AutoMapper;
using DataLibrary;
using Enums;
using Microsoft.Extensions.Logging;
using Models;
using ViewModels.Notification;

namespace Repositories.Shared.NotificationServices
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly ToranceContext _db;
        private readonly IMapper _mapper;

        public NotificationService(
            ILogger<NotificationService> logger,
            ToranceContext db,
            IMapper mapper
            )
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
        }

        public async Task<bool> AddNotificationAsync(NotificationViewModel model)
        {
            try
            {
                var notificationModel = _mapper.Map<Notification>(model);
                _db.Add(notificationModel);
                await _db.SaveChangesAsync();
                _logger.LogInformation("Notification has been sent!");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"AddNotification threw an exception, Message: {ex.Message}");
            }
            return false;
        }
    }
}
