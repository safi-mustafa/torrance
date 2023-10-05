using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repositories.Shared.UserInfoServices;

namespace Repositories.Shared.VersionService
{
    public class VersionService : IVersionService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;
        private readonly IUserInfoService _userInfoService;

        public VersionService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<VersionService> logger, IUserInfoService userInfoService)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _userInfoService = userInfoService;
        }

        public string GetVersionNumber()
        {
            var versionHeader = _configuration.GetValue<string>("VersionHeader");
            string version = "";
            if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey(versionHeader))
            {
                version = _httpContextAccessor.HttpContext.Request.Headers[versionHeader].ToString() ?? "0.0.0";
            }

            _logger.LogInformation($"LoggedInUserId: {_userInfoService.LoggedInUserId()}, LoggedInUser: {_userInfoService.LoggedInUserFullName()}, Version: {version}");
            return version;
        }

        public bool GetIsUpdateForcible()
        {
            var isUpdateForcible = _configuration.GetValue<bool>("IsUpdateForcible");
            return isUpdateForcible;
        }

        public string GetLatestApiVersion()
        {
            var version = _configuration.GetValue<string>("LatestVersion");
            if (string.IsNullOrEmpty(version))
                return "";
            return version;
        }
    }
}
