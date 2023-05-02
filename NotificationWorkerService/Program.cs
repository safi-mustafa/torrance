using Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.EventLog;
using NotificationWorkerService;
using NotificationWorkerService.Context;
using NotificationWorkerService.Interface;
using NotificationWorkerService.Repository;
using Repository;
using Serilog;

namespace NotificationWService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            try
            {
                Log.Information("Lab Notification Service has started");
                CreateHostBuilder(args).Build().Run();
                return;
            }
            catch (Exception ex)
            {
                Log.Fatal("There was an exception", ex);
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;
                    var connectionString = configuration.GetConnectionString("BainBridgeConnection");

                    services.AddDbContextFactory<NotificationDbContext>(config =>
                    {
                        config.UseSqlServer(connectionString);
                    });

                    services.AddHostedService<NotificationWorker>()
                    .Configure<EventLogSettings>(config =>
                    {
                        config.LogName = "Worker Logging Demo";
                        config.SourceName = "Event Logging Source";
                    });
                    services.AddDbContext<NotificationDbContext>();
                    services.AddSingleton<IEmail, EmailService>();
                    services.AddSingleton<ISms, SmsService>();
                    services.AddSingleton<IPushNotification, PushNotificationService>();
                })
                .UseSerilog();
    }
}
