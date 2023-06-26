using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Helpers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.WeldingRodRecord;
using Pagination;
using Repositories.Services.AppSettingServices.ApproverService;
using Repositories.Shared.AuthenticationService;
using Repositories.Shared.UserInfoServices;
using System.IdentityModel.Tokens.Jwt;
using ViewModels.Authentication.User;
using System.Security.Claims;
using System.Text;
using ViewModels.Authentication;
using ViewModels.Authentication.Approver;
using ViewModels.Shared;
using ViewModels.WeldingRodRecord.Employee;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Enums;
using Repositories.Services.AppSettingServices.EmployeeService;
using Repositories.Services.AppSettingServices.CompanyManagerService;
using ViewModels.AppSettings.CompanyManager;
using Centangle.Common.ResponseHelpers.Error;
using DocumentFormat.OpenXml.Spreadsheet;
using Repositories.Shared.VersionService;

namespace Torrance.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : TorranceController
    {
        public IConfiguration _configuration;
        private readonly ToranceContext _db;
        private readonly ILogger<AccountController> _logger;
        private readonly IRepositoryResponse _response;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identity;
        private readonly UserManager<ToranceUser> _userManager;
        private readonly SignInManager<ToranceUser> _signInManager;
        private readonly IUserInfoService _userInfoService;
        private readonly IApproverService<ApproverModifyViewModel, ApproverModifyViewModel, ApproverDetailViewModel> _approverService;
        private readonly IEmployeeService<EmployeeModifyViewModel, EmployeeModifyViewModel, EmployeeDetailViewModel> _employeeService;
        private readonly ICompanyManagerService<CompanyManagerModifyViewModel, CompanyManagerModifyViewModel, CompanyManagerDetailViewModel> _companyManagerService;
        private readonly IVersionService _versionService;

        public AccountController
            (
                IConfiguration configuration,
                ToranceContext db,
                ILogger<AccountController> logger,
                IRepositoryResponse response,
                IMapper mapper,
                IIdentityService identity,
                UserManager<ToranceUser> userManager,
                SignInManager<ToranceUser> signInManager,
                IUserInfoService userInfoService,
                IApproverService<ApproverModifyViewModel, ApproverModifyViewModel, ApproverDetailViewModel> approverService,
                IEmployeeService<EmployeeModifyViewModel, EmployeeModifyViewModel, EmployeeDetailViewModel> employeeService,
                ICompanyManagerService<CompanyManagerModifyViewModel, CompanyManagerModifyViewModel, CompanyManagerDetailViewModel> companyManagerService,
                IVersionService versionService
            )
        {
            _configuration = configuration;
            _db = db;
            _logger = logger;
            _response = response;
            _mapper = mapper;
            _identity = identity;
            _userManager = userManager;
            _signInManager = signInManager;
            _userInfoService = userInfoService;
            _approverService = approverService;
            _employeeService = employeeService;
            _companyManagerService = companyManagerService;
            this._versionService = versionService;
        }



        [HttpPost]
        [Route("/api/Account/LoginUsingPincode")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUsingPincode(LoginBriefVM model)
        {
            IRepositoryResponse result;
            try
            {
                _logger.LogInformation("Entered Login Endpoint", "login method 1");
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("Model State is valid", "login method 2");
                    var encodedPinCode = model.Pincode.EncodePasswordToBase64();
                    var user = await _db.Users.Where(x => x.AccessCode == encodedPinCode && x.IsDeleted == false).FirstOrDefaultAsync();
                    if (user != null)
                    {
                        if (user.ActiveStatus == ActiveStatus.Inactive || user.ActiveStatus == 0)
                        {
                            ModelState.AddModelError("pincode", "You can't login. Your status is inactive.");
                            result = Centangle.Common.ResponseHelpers.Response.BadRequestResponse(_response);
                            return ReturnProcessedResponse(result);
                        }

                        return await SetClaimns(model.DeviceId, user);
                    }
                    else
                    {
                        result = Centangle.Common.ResponseHelpers.Response.NotFoundResponse(_response);
                        return ReturnProcessedResponse(result);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("pincode", ex.Message);
                _logger.LogError(ex.Message, "Login Endpoint Exception msg");
            }
            result = Centangle.Common.ResponseHelpers.Response.BadRequestResponse(_response);
            return ReturnProcessedResponse(result);
        }

        [HttpPost]
        [Route("/api/Account/Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginVM model)
        {
            IRepositoryResponse result;
            try
            {
                _logger.LogInformation("Entered Login Endpoint", "login method 1");
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("Model State is valid", "login method 2");

                    var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Email == model.Email && u.IsDeleted == false);
                    if (user == null)
                    {
                        ModelState.AddModelError("Email", "Invalid login attempt.");
                        return ReturnProcessedResponse(Centangle.Common.ResponseHelpers.Response.NotFoundResponse(_response));
                    }

                    if (user.ActiveStatus == ActiveStatus.Inactive || user.ActiveStatus == 0)
                    {
                        ModelState.AddModelError("Email", "You can't login. Your status is inactive.");
                        result = Centangle.Common.ResponseHelpers.Response.BadRequestResponse(_response);
                        return ReturnProcessedResponse(result);
                    }

                    var response = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (user != null && response)
                    {
                        return await SetClaimns(model.DeviceId, user);
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Invalid login attempt.");
                        _logger.LogError("Invalid login attempt");
                        result = Centangle.Common.ResponseHelpers.Response.BadRequestResponse(_response);
                        return ReturnProcessedResponse(result);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Email", ex.Message);
                _logger.LogError(ex.Message, "Login Endpoint Exception msg");
            }
            result = Centangle.Common.ResponseHelpers.Response.BadRequestResponse(_response);
            return ReturnProcessedResponse(result);
        }

        [HttpPut]
        [Route("/api/Account/ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            IRepositoryResponse result;
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user == null)
                    {
                        ModelState.AddModelError("Email", "User does not exist.");
                        return ReturnProcessedResponse(Centangle.Common.ResponseHelpers.Response.BadRequestResponse(_response));
                    }
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var resetResult = await _userManager.ResetPasswordAsync(user, code, model.Password);
                    user.ChangePassword = false;
                    await _userManager.UpdateAsync(user);
                    if (resetResult.Succeeded)
                    {
                        return ReturnProcessedResponse(_response);
                    }
                    else
                    {
                        ErrorsHelper.AddErrorsToModelState(resetResult, ModelState, "Password");
                    }

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Email", ex.Message);
                _logger.LogError(ex.Message, "Login Endpoint Exception msg");
            }
            result = Centangle.Common.ResponseHelpers.Response.BadRequestResponse(_response);
            return ReturnProcessedResponse(result);
        }

        [Authorize]
        [HttpPost]
        [Route("/api/Account/Logout")]
        public async Task Logout(string deviceId)
        {
            var loggedInUserId = long.Parse(_userInfoService.LoggedInUserId());
            var user = await _db.Users.Where(x => (deviceId != null && x.DeviceId == deviceId) || (x.Id == loggedInUserId)).FirstOrDefaultAsync();
            if (user != null)
            {
                user.DeviceId = null;
                await _db.SaveChangesAsync();
            }
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
        }

        [Authorize]
        [HttpGet]
        [Route("/api/Account/GetAllUsers")]
        public async Task<IActionResult> GetAllUsers([FromQuery] UserSearchViewModel model)
        {
            var result = await _identity.GetAll<UserDetailViewModel>(model);
            var responseModel = new RepositoryResponseWithModel<PaginatedResultModel<UserDetailViewModel>>();
            responseModel.ReturnModel = result;
            return ReturnProcessedResponse<PaginatedResultModel<UserDetailViewModel>>(responseModel);
        }

        private async Task<IActionResult> SetClaimns(string deviceId, ToranceUser user)
        {
            IRepositoryResponse result;
            if (user?.ActiveStatus == Enums.ActiveStatus.Inactive)
            {
                ModelState.AddModelError("pincode", "Approval for this account is still pending.");
                _logger.LogError("Approval for this account is still pending.");

                result = Centangle.Common.ResponseHelpers.Response.BadRequestResponse(_response);
                return ReturnProcessedResponse(result);
            }

            if (user != null)
            {
                user.DeviceId = deviceId;
                await _db.SaveChangesAsync();
                var name = User?.Identity?.Name;
                _logger.LogInformation("User logged in.", "login method 4");

                var userRoles = await _userManager.GetRolesAsync(user);
                var role = userRoles.First();
                var companyId = user.CompanyId == null ? "0" : user.CompanyId.ToString();
                var authClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Role, role),
                            new Claim("FullName", user.FullName),
                            new Claim("CompanyId", companyId),
                            new Claim("ShowLogForms", user.CanAddLogs.ToString()),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                        };
                return ReturnProcessedResponse<UserTokenVM<BaseCrudViewModel>>(await GetResponseWithUserDetail(user, role, authClaims));
            }

            else
            {
                ModelState.AddModelError("pincode", "Invalid login attempt");
                _logger.LogError("Invalid login attempt");
                result = Centangle.Common.ResponseHelpers.Response.BadRequestResponse(_response);
                return ReturnProcessedResponse(result);
            }
        }

        #region Helping Methods

        private async Task<RepositoryResponseWithModel<UserTokenVM<BaseCrudViewModel>>> GetResponseWithUserDetail(ToranceUser? user, string role, List<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken
                (
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddYears(1),
                    claims: claims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
               );

            var responseModel = new RepositoryResponseWithModel<UserTokenVM<BaseCrudViewModel>>();
            responseModel.ReturnModel = new UserTokenVM<BaseCrudViewModel>
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiry = token.ValidTo
            };
            UserDetailViewModel userDetail;
            IRepositoryResponse response;
            if (role == RolesCatalog.Employee.ToString())
            {
                response = await _employeeService.GetById(user.Id);
                var parsedResponse = response as RepositoryResponseWithModel<EmployeeDetailViewModel>;
                userDetail = parsedResponse?.ReturnModel ?? new();

            }
            else if (role == RolesCatalog.CompanyManager.ToString())
            {
                response = await _companyManagerService.GetById(user.Id);
                var parsedResponse = response as RepositoryResponseWithModel<CompanyManagerDetailViewModel>;
                userDetail = parsedResponse?.ReturnModel ?? new();
            }
            else
            {
                response = await _approverService.GetById(user.Id);
                var parsedResponse = response as RepositoryResponseWithModel<ApproverDetailViewModel>;
                userDetail = parsedResponse?.ReturnModel ?? new();
            }

            responseModel.ReturnModel.UserDetail = userDetail;
            userDetail.Id = user.Id;
            userDetail.Role = role;
            return responseModel;
        }


        #endregion
    }
}
