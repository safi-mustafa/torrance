using Enums;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using NotificationWorkerService.Context;

namespace NotificationWorkerService;

public class NotificationWorker : BackgroundService
{
    private readonly ILogger<NotificationWorker> _logger;
    private readonly NotificationDbContext _db;
    private readonly IConfiguration _configuration;
    private readonly IEmail _emailService;
    private readonly ISms _smsService;

    public NotificationWorker(ILogger<NotificationWorker> logger, IConfiguration configuration, IEmail emailService, ISms smsService, IServiceScopeFactory factory)
    {
        _logger = logger;
        _configuration = configuration;
        _emailService = emailService;
        _smsService = smsService;
        _db = factory.CreateScope().ServiceProvider.GetRequiredService<NotificationDbContext>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Notification Worker running at: {time}", DateTimeOffset.Now);

            await SendNotifications();
            await Task.Delay((int)8.64e+7, stoppingToken);
        }
    }

    public async Task<bool> SendNotifications()
    {
        try
        {
            var currentDate = DateTime.Now;
            var notifications = await _db.Notifications.Where(x => x.ResendCount < 3 && x.SendTo != null && (x.CreatedOn.Date <= currentDate.Date && !x.IsSent)).ToListAsync();
            var emails = notifications.Where(x => x.Type == NotificationType.Email).ToList();
            var smss = notifications.Where(x => x.Type == NotificationType.Sms).ToList();
            var appEmail = _configuration["AppEmail"];
            foreach (var email in emails)
            {
                var emailResult = await _emailService.SendEmail(email.SendTo, appEmail, email.Subject, email.Message);
                if (emailResult)
                {
                    email.IsSent = true;
                }
                else
                {
                    email.IsSent = false;
                   // email.ResendCount += 1;
                }
            }
            foreach (var sms in smss)
            {
                var smsResult = await _smsService.SendSms(sms.SendTo, sms.Message);
                if (smsResult)
                    sms.IsSent = true;
                else
                {
                    sms.IsSent = false;
                 //   sms.ResendCount += 1;
                }
            }
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
        }
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        return false;
       
    }
}

