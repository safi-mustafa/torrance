﻿using Centangle.Common.ResponseHelpers.Models;
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
using Torrance.Api.Mapper;
using Centangle.Common.RequestHelpers.SwaggerFilters;
using Repositories.Services.CommonServices.ContractorService;
using Repositories.Services.CommonServices.DepartmentService;
using Repositories.Services.CommonServices.UnitService;
using Repositories.Services.TimeOnToolServices.PermitTypeService;
using Repositories.Services.TimeOnToolServices.ReworkService;
using Repositories.Services.TimeOnToolServices.ShiftDelayService;
using Repositories.Services.TimeOnToolServices.ShiftService;
using Repositories.Services.TimeOnToolServices.SOWService;
using Repositories.Services.TimeOnToolServices.TOTLogService;
using Repositories.Services.AppSettingServices.LocationService;
using Repositories.Services.AppSettingServices.RodTypeService;
using Repositories.Services.AppSettingServices.WeldMethodService;
using Repositories.Services.AppSettingServices.WRRLogService;
using Repositories.Shared.AuthenticationService;
using Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Repositories.Services.TimeOnToolServices.PermittingIssueService;
using Repositories.Shared.UserInfoServices;
using Helpers.File;
using Repositories.Shared.AttachmentService;
using Repositories.Services.FolderService;
using Repositories.Services.AppSettingServices.DropboxServices;
using Repositories.Services.OverrideLogServices.CraftSkillService;
using Repositories.Services.OverrideLogServices.LeadPlannerService;
using Repositories.Services.OverrideLogServices.OverrideTypeService;
using Repositories.Services.OverrideLogServices.ORLogService;
using Repositories.Services.CommonServices.CompanyService;
//using Repositories.Services.OverrideLogServices.CraftRateService;
using Repositories.Services.OverrideLogServices.ReasonForRequestService;
using Repositories.Services.TimeOnToolServices.DelayTypeService;
using Repositories.Shared.NotificationServices;
using Repositories.Services.AppSettingServices.ApproverService;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Repositories.Services.CommonServices.ApprovalService.Interface;
using Repositories.Services.CommonServices.ApprovalService;
using Repositories.Services.AppSettingServices.EmployeeService;
using Repositories.Services.AppSettingServices.CompanyManagerService;
using Repositories.Services.CommonServices.UserService;
using Repositories.Services.TimeOnToolServices.StartOfWorkDelayService;
using Repositories.Services.CommonServices.PossibleApproverService;
using Repositories.Shared.VersionService;
using Repositories.Services.TimeOnToolServices.OngoingWorkDelayService;
using Repositories.Services.DashboardService;
using CorrelationId.DependencyInjection;
using System.Security.Claims;

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
                    ClockSkew = TimeSpan.FromMinutes(5),

                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        try
                        {
                            var strId = context.Principal?.Claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
                            int.TryParse(strId, out int id);
                            if (id > 0)
                            {
                                var serviceProvider = context.HttpContext.RequestServices;
                                var dbContext = serviceProvider.GetRequiredService<ToranceContext>(); // Replace YourDbContext with your actual DbContext type
                                var user = await dbContext.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
                                if (user == null || user.IsDeleted == true || user.ActiveStatus == Enums.ActiveStatus.Inactive)
                                {
                                    context.Fail("User is deleted or In-active.");
                                    return;
                                }
                            }
                            else
                            {
                                context.Fail("No such user exists.");
                                return;
                            }

                            var currentVersion = configuration["JWT:Version"] ?? "null"; // Default version if not specified in settings
                            var tokenVersionClaim = context.Principal?.Claims?.FirstOrDefault(claim => claim.Type == "Version")?.Value;
                            if (tokenVersionClaim == null)
                            {
                                tokenVersionClaim = "null";
                            }
                            // Compare token version with the stored version
                            if (tokenVersionClaim != currentVersion)
                            {
                                context.Fail("Token is using an outdated version.");
                            }

                        }
                        catch (Exception ex)
                        {
                            // Handle exceptions appropriately, log them, etc.
                            context.Fail($"An error occurred: {ex.Message}");
                        }
                        return;
                    }
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

            //services.AddAutoMapper(typeof(Mapping).Assembly);

            // Register the worker responsible of seeding the database with the sample clients.
            // Note: in a real world application, this step should be part of a setup script.
            //services.AddHostedService<SeedWorker>();

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                // options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
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
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IRepositoryResponse, RepositoryResponse>();
            services.AddScoped<IUserStore<ToranceUser>, UserStore<ToranceUser, ToranceRole, ToranceContext, long>>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped(typeof(IORLogService<,,>), typeof(ORLogService<,,>));
            services.AddScoped(typeof(IContractorService<,,>), typeof(ContractorService<,,>));
            services.AddScoped(typeof(ICompanyService<,,>), typeof(CompanyService<,,>));
            services.AddScoped(typeof(IDepartmentService<,,>), typeof(DepartmentService<,,>));
            services.AddScoped(typeof(IUnitService<,,>), typeof(UnitService<,,>));
            services.AddScoped(typeof(IPermitTypeService<,,>), typeof(PermitTypeService<,,>));
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
            services.AddScoped(typeof(ICompanyManagerService<,,>), typeof(CompanyManagerService<,,>));
            services.AddScoped(typeof(IWRRLogService<,,>), typeof(WRRLogService<,,>));
            services.AddScoped(typeof(ITOTLogService<,,>), typeof(TOTLogService<,,>));
            services.AddScoped(typeof(IDropboxService<,,>), typeof(DropboxService<,,>));
            services.AddScoped(typeof(IFolderService<,,>), typeof(FolderService<,,>));
            services.AddScoped(typeof(IAttachmentService<,,>), typeof(AttachmentService<,,>));
            //services.AddScoped(typeof(ICraftRateService<,,>), typeof(CraftRateService<,,>));
            services.AddScoped(typeof(ICraftSkillService<,,>), typeof(CraftSkillService<,,>));
            services.AddScoped(typeof(ILeadPlannerService<,,>), typeof(LeadPlannerService<,,>));
            services.AddScoped(typeof(IOverrideTypeService<,,>), typeof(OverrideTypeService<,,>));
            services.AddScoped(typeof(IReasonForRequestService<,,>), typeof(ReasonForRequestService<,,>));
            services.AddScoped(typeof(IDelayTypeService<,,>), typeof(DelayTypeService<,,>));
            services.AddScoped(typeof(IApproverService<,,>), typeof(ApproverService<,,>));
            services.AddScoped(typeof(IPossibleApproverService), typeof(PossibleApproverService));
            services.AddScoped<IApprovalService, ApprovalService>();
            services.AddScoped(typeof(IUserService<,,>), typeof(UserService<,,>));
            services.AddScoped<IFileHelper, FileHelper>();
            services.AddScoped<IUserInfoService, UserInfoService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IVersionService, VersionService>();
            services.AddScoped(typeof(IOngoingWorkDelayService<,,>), typeof(OngoingWorkDelayService<,,>));
            services.AddDefaultCorrelationId();

        }

    }
}
