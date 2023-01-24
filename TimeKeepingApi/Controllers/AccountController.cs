using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pagination;
using Repositories.Shared.AuthenticationService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ViewModels.Authentication;
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

        public AccountController
            (
                IConfiguration configuration, 
                ToranceContext db, 
                ILogger<AccountController> logger, 
                IRepositoryResponse response,
                IMapper mapper,
                IIdentityService identity
            )
        {
            _configuration = configuration;
            _db = db;
            _logger = logger;
            _response = response;
            _mapper = mapper;
            _identity = identity;
        }

        [HttpPost]
        [Route("/api/Account/Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string pincode)
        {
            IRepositoryResponse result;
            try
            {
                _logger.LogInformation("Entered Login Endpoint", "login method 1");
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("Model State is valid", "login method 2");
                    var user = await _db.Employees.Where(x => x.EmployeeId == pincode).FirstOrDefaultAsync();
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
                        var name = User?.Identity?.Name;
                        _logger.LogInformation("User logged in.", "login method 4");

                        var fullName = $"{user?.FirstName} {user?.LastName}";

                        var authClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Name, user.FirstName),
                            new Claim("FullName", fullName),
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

                        var responseModel = new RepositoryResponseWithModel<TokenVM>();
                        responseModel.ReturnModel = new TokenVM
                        {
                            Token = new JwtSecurityTokenHandler().WriteToken(token),
                            Expiry = token.ValidTo,
                            UserDetail = userDetail
                        };
                        return ReturnProcessedResponse<TokenVM>(responseModel);
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
            result = Centangle.Common.ResponseHelpers.Response.UnAuthorizedResponse(_response);
            return ReturnProcessedResponse(result);
        }

        [HttpGet]
        [Route("/api/Account/GetAllUsers")]
        public async Task<IActionResult> GetAllUsers([FromQuery] UserSearchViewModel model)
        {
            var result = await _identity.GetAll<UserDetailVM>(model);
            var responseModel = new RepositoryResponseWithModel<PaginatedResultModel<UserDetailVM>>();
            responseModel.ReturnModel = result;
            return ReturnProcessedResponse<PaginatedResultModel<UserDetailVM>>(responseModel);
        }
    }
}
