using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models;
using Pagination;
using Repositories.Shared.AuthenticationService;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using ViewModels.Authentication;
using ViewModels.Authentication.Approver;
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

        public AccountController
            (
                IConfiguration configuration,
                ToranceContext db,
                ILogger<AccountController> logger,
                IRepositoryResponse response,
                IMapper mapper,
                IIdentityService identity,
                UserManager<ToranceUser> userManager
            )
        {
            _configuration = configuration;
            _db = db;
            _logger = logger;
            _response = response;
            _mapper = mapper;
            _identity = identity;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("/api/Account/LoginUsingPincode")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUsingPincode(string pincode)
        {
            IRepositoryResponse result;
            try
            {
                _logger.LogInformation("Entered Login Endpoint", "login method 1");
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("Model State is valid", "login method 2");
                    var user = await _db.Employees.Include(x => x.Contractor).Where(x => x.EmployeeId == pincode).FirstOrDefaultAsync();
                    if (user?.ActiveStatus == Enums.ActiveStatus.Inactive)
                    {
                        ModelState.AddModelError(string.Empty, "Approval for this account is still pending.");
                        _logger.LogError("Approval for this account is still pending.");

                        result = Centangle.Common.ResponseHelpers.Response.BadRequestResponse(_response);
                        var check = ReturnProcessedResponse(result);
                        return check;
                    }

                    if (user != null)
                    {
                        var aspNetUser = await _db.Users.Where(x => x.Id == user.UserId).FirstOrDefaultAsync();
                        var name = User?.Identity?.Name;
                        _logger.LogInformation("User logged in.", "login method 4");

                        var fullName = $"{user?.FirstName} {user?.LastName}";
                        var userRoles = await _userManager.GetRolesAsync(aspNetUser);
                        var role = userRoles.First();

                        var authClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Name, user.FirstName),
                            new Claim("FullName", fullName),
                            new Claim("EmployeeId", user.Id.ToString()),
                            new Claim(ClaimTypes.Role, role),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                        var token = new JwtSecurityToken
                            (
                                issuer: _configuration["JWT:ValidIssuer"],
                                audience: _configuration["JWT:ValidAudience"],
                                expires: DateTime.Now.AddHours(12),
                                claims: authClaims,
                                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                           );

                        var userDetail = _mapper.Map<EmployeeDetailViewModel>(user);

                        var responseModel = new RepositoryResponseWithModel<EmployeeTokenVM>();
                        responseModel.ReturnModel = new EmployeeTokenVM
                        {
                            Token = new JwtSecurityTokenHandler().WriteToken(token),
                            Expiry = token.ValidTo,
                            UserDetail = userDetail
                        };
                        return ReturnProcessedResponse<EmployeeTokenVM>(responseModel);
                    }

                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        _logger.LogError("Invalid login attempt");
                        result = Centangle.Common.ResponseHelpers.Response.BadRequestResponse(_response);
                        return ReturnProcessedResponse(result);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
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

                    var user = await _userManager.FindByEmailAsync(model.Email);
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

                        var userDetail = _mapper.Map<UserDetailViewModel>(user);
                        userDetail.Roles = userRoles.Select(x => new UserRolesVM { Name = x }).ToList();
                        userDetail.Role = role;
                        //  userDetail.Roles = string.Join(',', userRoles);

                        var responseModel = new RepositoryResponseWithModel<UserTokenVM>();
                        responseModel.ReturnModel = new UserTokenVM
                        {
                            Token = new JwtSecurityTokenHandler().WriteToken(token),
                            Expiry = token.ValidTo,
                            UserDetail = userDetail
                        };
                        return ReturnProcessedResponse<UserTokenVM>(responseModel);
                    }

                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        _logger.LogError("Invalid login attempt");
                        result = Centangle.Common.ResponseHelpers.Response.BadRequestResponse(_response);
                        return ReturnProcessedResponse(result);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                _logger.LogError(ex.Message, "Login Endpoint Exception msg");
            }
            result = Centangle.Common.ResponseHelpers.Response.BadRequestResponse(_response);
            return ReturnProcessedResponse(result);
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
