﻿using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Helpers.File;
using IdentityProvider.Data.IdentityStore;
using IdentityProvider.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Models;
using Models.Mapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repositories.Services.AppSettingServices.DropboxServices;
using Repositories.Services.AppSettingServices.EmployeeService;
using Repositories.Services.AppSettingServices.MappService;
using Repositories.Services.CommonServices.ApprovalService;
using Repositories.Services.CommonServices.ApprovalService.Interface;
using Repositories.Services.CommonServices.CompanyService;
using Repositories.Services.CommonServices.ContractorService;
using Repositories.Services.CommonServices.DepartmentService;
using Repositories.Services.CommonServices.UnitService;
using Repositories.Services.DashboardService;
using Repositories.Services.FolderService;
//using Repositories.Services.OverrideLogServices.CraftRateService;
using Repositories.Services.OverrideLogServices.CraftSkillService;
using Repositories.Services.OverrideLogServices.LeadPlannerService;
using Repositories.Services.OverrideLogServices.ORLogService;
using Repositories.Services.OverrideLogServices.OverrideTypeService;
using Repositories.Services.OverrideLogServices.ReasonForRequestService;
using Repositories.Services.TimeOnToolServices.DelayTypeService;
using Repositories.Services.TimeOnToolServices.PermittingIssueService;
using Repositories.Services.TimeOnToolServices.PermitTypeService;
using Repositories.Services.TimeOnToolServices.ReworkService;
using Repositories.Services.TimeOnToolServices.ShiftDelayService;
using Repositories.Services.TimeOnToolServices.ShiftService;
using Repositories.Services.TimeOnToolServices.SOWService;
using Repositories.Services.TimeOnToolServices.TOTLogService;
using Repositories.Services.AppSettingServices.ApproverService;
using Repositories.Services.AppSettingServices.LocationService;
using Repositories.Services.AppSettingServices.RodTypeService;
using Repositories.Services.AppSettingServices.WeldMethodService;
using Repositories.Services.AppSettingServices.WRRLogService;
using Repositories.Shared.AttachmentService;
using Repositories.Shared.AuthenticationService;
using Repositories.Shared.NotificationServices;
using Repositories.Shared.UserInfoServices;
using Repositories.Services.CommonServices.UserService;
using Repositories.Services.AppSettingServices.CompanyManagerService;
using Repositories.Services.AppSettingServices.ForemanService;
using Repositories.Services.TimeOnToolServices.StartOfWorkDelayService;
using Repositories.Services.AppSettingServices.AdministratorService;
using Repositories.Services.CommonServices.PossibleApproverService;
using Repositories.Services.TimeOnToolServices.OngoingWorkDelayService;
using CorrelationId.DependencyInjection;

namespace Web.Extensions
{
    public static class StartupExtension
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("TorranceConnection") ?? throw new InvalidOperationException("Connection string 'TorranceConnection' not found.");

            services.AddDbContext<ToranceContext>(options =>
                options
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                .UseSqlServer(connectionString));
            services.AddAutoMapper(typeof(Mapping));

            services.AddDefaultIdentity<ToranceUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddUserStore<UserStore<ToranceUser, ToranceRole, ToranceContext, long>>()
                .AddRoles<ToranceRole>()
                .AddRoleManager<RoleManager<ToranceRole>>()
                .AddEntityFrameworkStores<ToranceContext>()
                .AddSignInManager<SignInManager<ToranceUser>>()
                .AddClaimsPrincipalFactory<ApplicationUserClaimsPrincipalFactory>()
                .AddDefaultTokenProviders()
                 .AddDefaultUI();


            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 0;
                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
                //Sign in settings.
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
        }
        public static void ConfigureDependencies(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryResponse, RepositoryResponse>();
            services.AddScoped<IUserStore<ToranceUser>, UserStore<ToranceUser, ToranceRole, ToranceContext, long>>();
            services.AddHostedService<SeedWorker>();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped(typeof(IContractorService<,,>), typeof(ContractorService<,,>));
            services.AddScoped(typeof(ICompanyService<,,>), typeof(CompanyService<,,>));
            services.AddScoped(typeof(IDepartmentService<,,>), typeof(DepartmentService<,,>));
            services.AddScoped(typeof(IUnitService<,,>), typeof(UnitService<,,>));
            services.AddScoped(typeof(IPermitTypeService<,,>), typeof(PermitTypeService<,,>));
            services.AddScoped(typeof(IDelayTypeService<,,>), typeof(DelayTypeService<,,>));
            services.AddScoped(typeof(IPermittingIssueService<,,>), typeof(PermittingIssueService<,,>));
            services.AddScoped(typeof(IReworkDelayService<,,>), typeof(ReworkDelayService<,,>));
            services.AddScoped(typeof(IShiftDelayService<,,>), typeof(ShiftDelayService<,,>));
            services.AddScoped(typeof(IStartOfWorkDelayService<,,>), typeof(StartOfWorkDelayService<,,>));
            services.AddScoped(typeof(IShiftService<,,>), typeof(ShiftService<,,>));
            services.AddScoped(typeof(ISOWService<,,>), typeof(SOWService<,,>));
            services.AddScoped(typeof(ILocationService<,,>), typeof(LocationService<,,>));
            services.AddScoped(typeof(IWeldMethodService<,,>), typeof(WeldMethodService<,,>));
            services.AddScoped(typeof(IRodTypeService<,,>), typeof(RodTypeService<,,>));
            services.AddScoped(typeof(IEmployeeService<,,>), typeof(EmployeeService<,,>));
            services.AddScoped(typeof(IAdministratorService<,,>), typeof(AdministratorService<,,>));
            services.AddScoped(typeof(ICompanyManagerService<,,>), typeof(CompanyManagerService<,,>));
            services.AddScoped(typeof(IForemanService<,,>), typeof(ForemanService<,,>));
            services.AddScoped(typeof(IWRRLogService<,,>), typeof(WRRLogService<,,>));
            services.AddScoped(typeof(ITOTLogService<,,>), typeof(TOTLogService<,,>));
            services.AddScoped<IPossibleApproverService, PossibleApproverService>();
            services.AddScoped(typeof(IMapService<,,>), typeof(MapService<,,>));
            services.AddScoped(typeof(IDropboxService<,,>), typeof(DropboxService<,,>));
            services.AddScoped<IFileHelper, FileHelper>();

            services.AddScoped(typeof(IFolderService<,,>), typeof(FolderService<,,>));
            services.AddScoped(typeof(IAttachmentService<,,>), typeof(AttachmentService<,,>));
            services.AddScoped(typeof(IApproverService<,,>), typeof(ApproverService<,,>));
            services.AddScoped(typeof(IApprovalService), typeof(ApprovalService));
            //services.AddScoped(typeof(ICraftRateService<,,>), typeof(CraftRateService<,,>));
            services.AddScoped(typeof(ICraftSkillService<,,>), typeof(CraftSkillService<,,>));
            services.AddScoped(typeof(ILeadPlannerService<,,>), typeof(LeadPlannerService<,,>));
            services.AddScoped(typeof(IOverrideTypeService<,,>), typeof(OverrideTypeService<,,>));
            services.AddScoped(typeof(IReasonForRequestService<,,>), typeof(ReasonForRequestService<,,>));
            services.AddScoped(typeof(IORLogService<,,>), typeof(ORLogService<,,>));
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped(typeof(IUserService<,,>), typeof(UserService<,,>));
            //services.AddScoped<IUserService, UserService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IUserInfoService, UserInfoService>();
            services.AddScoped(typeof(IOngoingWorkDelayService<,,>), typeof(OngoingWorkDelayService<,,>));
            services.AddDefaultCorrelationId();

        }
    }
}
