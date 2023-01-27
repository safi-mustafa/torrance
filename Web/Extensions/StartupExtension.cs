using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Helpers.File;
using IdentityProvider.Data.IdentityStore;
using IdentityProvider.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Mapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repositories.Services.AppSettingServices.DropboxServices;
using Repositories.Services.AppSettingServices.MappService;
using Repositories.Services.CommonServices.ContractorService;
using Repositories.Services.CommonServices.DepartmentService;
using Repositories.Services.CommonServices.UnitService;
using Repositories.Services.FolderService;
using Repositories.Services.TimeOnToolServices.PermittingIssueService;
using Repositories.Services.TimeOnToolServices.PermitTypeService;
using Repositories.Services.TimeOnToolServices.ReworkService;
using Repositories.Services.TimeOnToolServices.ShiftDelayService;
using Repositories.Services.TimeOnToolServices.ShiftService;
using Repositories.Services.TimeOnToolServices.SOWService;
using Repositories.Services.TimeOnToolServices.TOTLogService;
using Repositories.Services.TimeOnToolServices.UserService;
using Repositories.Services.WeldRodRecordServices.EmployeeService;
using Repositories.Services.WeldRodRecordServices.LocationService;
using Repositories.Services.WeldRodRecordServices.RodTypeService;
using Repositories.Services.WeldRodRecordServices.WeldMethodService;
using Repositories.Services.WeldRodRecordServices.WRRLogService;
using Repositories.Shared.AttachmentService;
using Repositories.Shared.AuthenticationService;
using Repositories.Shared.UserInfoServices;

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

            //services.AddAuthorization();
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
            services.AddScoped(typeof(IContractorService<,,>), typeof(ContractorService<,,>));
            services.AddScoped(typeof(IDepartmentService<,,>), typeof(DepartmentService<,,>));
            services.AddScoped(typeof(IUnitService<,,>), typeof(UnitService<,,>));
            services.AddScoped(typeof(IPermitTypeService<,,>), typeof(PermitTypeService<,,>));
            services.AddScoped(typeof(IPermittingIssueService<,,>), typeof(PermittingIssueService<,,>));
            services.AddScoped(typeof(IReworkDelayService<,,>), typeof(ReworkDelayService<,,>));
            services.AddScoped(typeof(IShiftDelayService<,,>), typeof(ShiftDelayService<,,>));
            services.AddScoped(typeof(IShiftService<,,>), typeof(ShiftService<,,>));
            services.AddScoped(typeof(ISOWService<,,>), typeof(SOWService<,,>));
            services.AddScoped(typeof(ILocationService<,,>), typeof(LocationService<,,>));
            services.AddScoped(typeof(IWeldMethodService<,,>), typeof(WeldMethodService<,,>));
            services.AddScoped(typeof(IRodTypeService<,,>), typeof(RodTypeService<,,>));
            services.AddScoped(typeof(IEmployeeService<,,>), typeof(EmployeeService<,,>));
            services.AddScoped(typeof(IWRRLogService<,,>), typeof(WRRLogService<,,>));
            services.AddScoped(typeof(ITOTLogService<,,>), typeof(TOTLogService<,,>));
            services.AddScoped(typeof(IMapService<,,>), typeof(MapService<,,>));
            services.AddScoped(typeof(IDropboxService<,,>), typeof(DropboxService<,,>));
            services.AddScoped<IFileHelper, FileHelper>();
            services.AddScoped(typeof(IFolderService<,,>), typeof(FolderService<,,>));
            services.AddScoped(typeof(IAttachmentService<,,>), typeof(AttachmentService<,,>));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IUserInfoService, UserInfoService>();
        }
    }
}
