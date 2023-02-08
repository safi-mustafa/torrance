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
using Repositories.Services.WeldRodRecordServices.ApproverService;
using Repositories.Shared.AuthenticationService;
using Repositories.Shared.UserInfoServices;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using ViewModels.Authentication;
using ViewModels.Authentication.Approver;
using ViewModels.Shared;
using ViewModels.WeldingRodRecord.Employee;

namespace TorranceApi.Controllers
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
                IApproverService<ApproverModifyViewModel, ApproverModifyViewModel, ApproverDetailViewModel> approverService
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

                    var user = await _db.Users.Where(x => x.AccessCode == encodedPinCode).FirstOrDefaultAsync();

                    if (user?.ActiveStatus == Enums.ActiveStatus.Inactive)
                    {
                        ModelState.AddModelError("pincode", "Approval for this account is still pending.");
                        _logger.LogError("Approval for this account is still pending.");

                        result = Centangle.Common.ResponseHelpers.Response.BadRequestResponse(_response);
                        var check = ReturnProcessedResponse(result);
                        return check;
                    }

                    if (user != null)
                    {

                        user.DeviceId = model.DeviceId;
                        await _db.SaveChangesAsync();
                        var name = User?.Identity?.Name;
                        _logger.LogInformation("User logged in.", "login method 4");

                        var userRoles = await _userManager.GetRolesAsync(user);
                        var role = userRoles.First();

                        var authClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Role, role),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                        };

                        var employee = await _db.Employees.Include(x => x.Company).Where(x => x.EmployeeId == model.Pincode).FirstOrDefaultAsync();
                        if (role == "Employee")
                        {
                            var employee = await _db.Employees.Include(x => x.Company).Where(x => x.EmployeeId == model.Pincode).FirstOrDefaultAsync();
                            var fullName = $"{employee?.FirstName} {employee?.LastName}";
                            authClaims.AddRange(new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, employee.FirstName),
                                new Claim("FullName", fullName),
                                new Claim("EmployeeId", employee.Id.ToString()),
                            });
                        }

                        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                        var token = new JwtSecurityToken
                            (
                                issuer: _configuration["JWT:ValidIssuer"],
                                audience: _configuration["JWT:ValidAudience"],
                                expires: DateTime.Now.AddHours(12),
                                claims: authClaims,
                                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                           );

                        RepositoryResponseWithModel<UserTokenVM<BaseCrudViewModel>> responseModel = await GetResponseWithUserDetailForPin(user, role, employee, token);
                        return ReturnProcessedResponse<ApproverTokenVM>(responseModel);
                    }

                    else
                    {
                        ModelState.AddModelError("pincode", "Invalid login attempt. The pincode is incorrect.");
                        _logger.LogError("Invalid login attempt");
                        result = Centangle.Common.ResponseHelpers.Response.BadRequestResponse(_response);
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

        private async Task<RepositoryResponseWithModel<UserTokenVM<BaseCrudViewModel>>> GetResponseWithUserDetailForPin(ToranceUser? user, string role, Employee? employee, JwtSecurityToken token)
        {
            var responseModel = new RepositoryResponseWithModel<UserTokenVM<BaseCrudViewModel>>();
            responseModel.ReturnModel = new UserTokenVM<BaseCrudViewModel>
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiry = token.ValidTo
            };
            if (role == "Employee")
            {
                var userDetail = _mapper.Map<EmployeeDetailViewModel>(employee);
                userDetail.Id = user.Id;// Temporary For TOT LOG
                userDetail.Role = role;
                responseModel.ReturnModel.UserDetail = userDetail;
            }
            else
            {
                var response = await _approverService.GetById(user.Id);
                var parsedResponse = response as RepositoryResponseWithModel<ApproverDetailViewModel>;
                var userDetail = parsedResponse?.ReturnModel ?? new();
                userDetail.Role = role;
                responseModel.ReturnModel.UserDetail = userDetail;
            }

            return responseModel;
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

                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user == null)
                    {
                        ModelState.AddModelError("Email", "Invalid login attempt. The Email is incorrect.");
                        return ReturnProcessedResponse(Centangle.Common.ResponseHelpers.Response.BadRequestResponse(_response));
                    }
                    //if (user?.IsApproved == false)
                    //{
                    //    ModelState.AddModelError(string.Empty, "Approval for this account is still pending.");
                    //    _logger.LogError("Approval for this account is still pending.");

                    //    result = Centangle.Common.ResponseHelpers.Response.BadRequestResponse(_response);
                    //    var check = ReturnProcessedResponse(result);
                    //    return check;
                    //}
                    var response = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (user != null && response)
                    {
                        user.DeviceId = model.DeviceId;
                        await _db.SaveChangesAsync();
                        var name = User?.Identity?.Name;
                        _logger.LogInformation("User logged in.", "login method 4");
                        var userRoles = await _userManager.GetRolesAsync(user);
                        var role = userRoles.First();
                        var authClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim("Role", role),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };
                        if (userRoles.Contains("Admin"))
                        {
                            authClaims.Add(new Claim(ClaimTypes.Role, "Admin"));
                        }
                        else
                        {
                            foreach (var userRole in userRoles)
                            {
                                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                            }
                        }

                        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                        var token = new JwtSecurityToken
                            (
                                issuer: _configuration["JWT:ValidIssuer"],
                                audience: _configuration["JWT:ValidAudience"],
                                expires: DateTime.Now.AddHours(12),
                                claims: authClaims,
                                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                           );
                        var responseModel = new RepositoryResponseWithModel<UserTokenVM<BaseCrudViewModel>>();
                        responseModel.ReturnModel = new UserTokenVM<BaseCrudViewModel>
                        {
                            Token = new JwtSecurityTokenHandler().WriteToken(token),
                            Expiry = token.ValidTo
                        };

                        if (role == "Approver")
                        {
                            var approverResponse = await _approverService.GetById(user.Id);
                            var parsedResponse = approverResponse as RepositoryResponseWithModel<ApproverDetailViewModel>;
                            var userDetail = parsedResponse?.ReturnModel ?? new();

                            userDetail.Roles = userRoles.Select(x => new UserRolesVM { Name = x }).ToList();
                            userDetail.Role = role;
                            responseModel.ReturnModel.UserDetail = userDetail;
                        }
                        else
                        {
                            var userDetail = _mapper.Map<UserDetailViewModel>(user);
                            userDetail.Roles = userRoles.Select(x => new UserRolesVM { Name = x }).ToList();
                            userDetail.Role = role;
                            responseModel.ReturnModel.UserDetail = userDetail;
                        }

                        return ReturnProcessedResponse<TokenVM>(responseModel);
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Invalid login attempt. The Password is incorrect.");
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
    }
}
