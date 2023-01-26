using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text;
using TorranceApi.Mapper;
using Centangle.Common.RequestHelpers.SwaggerFilters;
using Web.Extensions;
using Repositories.Services.CommonServices.ContractorService;
using Repositories.Services.CommonServices.DepartmentService;
using Repositories.Services.CommonServices.UnitService;
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
using Repositories.Shared.AuthenticationService;
using TorranceApi.Mapper;
using Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Repositories.Services.TimeOnToolServices.PermittingIssueService;
using Repositories.Shared.UserInfoServices;
using Repositories.Services.AppSettingServices.MobileFileServices;
using Helpers.File;
using Repositories.Shared.AttachmentService;

namespace Web.Extensions
{
    public static class StartupExtension
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ToranceContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("TorranceConnection"), b => b.MigrationsAssembly("DataLibrary"))
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
            });

            services.AddAutoMapper(typeof(Mapping));

            services.AddDefaultIdentity<ToranceUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddUserStore<UserStore<ToranceUser, ToranceRole, ToranceContext, long>>()
                .AddRoles<ToranceRole>()
                .AddRoleManager<RoleManager<ToranceRole>>()
                .AddEntityFrameworkStores<ToranceContext>()
                .AddSignInManager<SignInManager<ToranceUser>>()
                .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<ToranceUser>>()
                .AddDefaultTokenProviders()
                 .AddDefaultUI();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // Adding Jwt Bearer  
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                //options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
                    ClockSkew = TimeSpan.FromMinutes(5)
                };
            });
            services.AddAuthorization();

            services.AddCors(confg =>
                confg.AddPolicy("AllowAll",
                    p => p.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()));

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = true;
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

            //services.AddAutoMapper(typeof(Mapping).Assembly);

            // Register the worker responsible of seeding the database with the sample clients.
            // Note: in a real world application, this step should be part of a setup script.
            //services.AddHostedService<SeedWorker>();

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Torrance API", Version = "v1" });
                c.OperationFilter<SwaggerFileOperationFilter>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                    "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n " +
                    "Example: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });
            services.AddSwaggerGenNewtonsoftSupport();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }

        public static void ConfigureDependencies(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IRepositoryResponse, RepositoryResponse>();
            services.AddScoped<IUserStore<ToranceUser>, UserStore<ToranceUser, ToranceRole, ToranceContext, long>>();
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
            services.AddScoped(typeof(IMobileFileService<,,>), typeof(MobileFileService<,,>));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAttachmentService, AttachmentService>();
            services.AddScoped<IFileHelper, FileHelper>();
            services.AddScoped<IUserInfoService, UserInfoService>();
            services.AddScoped<IIdentityService, IdentityService>();
        }
    }
}
