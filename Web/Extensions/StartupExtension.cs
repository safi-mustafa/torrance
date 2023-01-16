using DataLibrary;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Mapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Web.Extensions
{
    public static class StartupExtension
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ChargieContextConnection") ?? throw new InvalidOperationException("Connection string 'ChargieContextConnection' not found.");

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
                .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<ToranceUser>>()
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
                options.Password.RequiredLength = 5;
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
    }
}
