using AutoMapper;
using DataLibrary;
using Helpers.Extensions;
using Helpers.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;
using Pagination;
using Repositories.Shared.NotificationServices;
using Repositories.Shared.UserInfoServices;
using ViewModels.Authentication;
using ViewModels.Authentication.User;
using ViewModels.Notification;

namespace Repositories.Shared.AuthenticationService
{
    public class IdentityService : IIdentityService
    {
        private readonly SignInManager<ToranceUser> _signInManager;
        private readonly UserManager<ToranceUser> _userManager;
        private readonly IUserStore<ToranceUser> _userStore;
        private readonly IUserEmailStore<ToranceUser> _emailStore;
        private readonly ILogger<IdentityService> _logger;
        private readonly IMapper _mapper;
        private readonly ToranceContext _db;
        private readonly IUserInfoService _userInfoService;
        private readonly INotificationService _notificationService;
        private readonly IActionContextAccessor _actionContext;

        public IdentityService(
            UserManager<ToranceUser> userManager,
            IUserStore<ToranceUser> userStore,
            SignInManager<ToranceUser> signInManager,
            ILogger<IdentityService> logger,
            IMapper mapper,
            ToranceContext db,
            IUserInfoService userInfoService,
            INotificationService notificationService,
            IActionContextAccessor actionContext
            )
        {
            _userManager = userManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _logger = logger;
            _mapper = mapper;
            _db = db;
            _userInfoService = userInfoService;
            _notificationService = notificationService;
            _actionContext = actionContext;
        }

        public async Task<long> CreateUser(SignUpModel model, string optionalUsernamePrefix = "")
        {
            try
            {
                var user = _mapper.Map<ToranceUser>(model);
                try
                {
                    user.UserName = Guid.NewGuid().ToString();
                    if (model.Role == "Administrator")
                    {
                        user.DisableNotifications = model.DisableNotifications;
                    }
                    var result = await _userManager.CreateAsync(user, model.Password);
                    var role = model.Role != null ? model.Role : "SuperAdmin";
                    if (result.Succeeded)
                    {
                        if ((await _userManager.AddToRoleAsync(user, role)).Succeeded)
                        {
                            return user.Id;
                        }
                    }
                    else
                    {
                        Errors.AddErrorsToModelState(result, _actionContext.ActionContext.ModelState);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "CraeteUser threw the following exception");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CraeteUser threw the following exception");
            }
            return -1;
        }


        public async Task<bool> UpdateUser(SignUpModel model)
        {
            string errorMsg = "There was some problem in updating data. Please try again later.";
            try
            {
                var id = model.Id.ToString();
                var user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    //user.UserName = model.UserName;
                    //user.Email = model.Email;
                    user.AccessCode = model.IsExcelSheet ? model.AccessCode.EncodePasswordToBase64() : user.AccessCode;
                    user.FullName = model.FullName;
                    user.CompanyId = model.Company?.Id;
                    user.Email = model.Email;
                    user.ActiveStatus = model.ActiveStatus;
                    user.NormalizedEmail = model.Email.ToUpper();
                    user.CanAddLogs = model.CanAddLogs;
                    if (model.Role == "Administrator")
                    {
                        user.DisableNotifications = model.DisableNotifications;
                    }
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(model.Password))
                        {
                            await _userManager.RemovePasswordAsync(user);
                            var response = await _userManager.AddPasswordAsync(user, model.Password);
                            if (response.Succeeded)
                            {
                                return result.Succeeded;
                            }
                            else
                            {
                                _logger.LogWarning("Error! Password not updating.");
                            }
                        }
                        return result.Succeeded;
                    }
                    else
                    {
                        Errors.AddErrorsToModelState(result, _actionContext.ActionContext.ModelState);
                        _logger.LogWarning(errorMsg, "Warning while updating user");
                    }
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateUser threw following exception.");
            }
            return false;
        }

        public async Task<PaginatedResultModel<T>> GetAll<T>(BaseSearchModel searchFilter)
        {
            try
            {
                var search = searchFilter as UserSearchViewModel;
                searchFilter.OrderByColumn = string.IsNullOrEmpty(search.OrderByColumn) ? "Id" : search.OrderByColumn;

                var rolesName = search.Roles.Select(x => x.Name).ToList();
                var userQueryable = (from user in _db.Users
                                     join userRole in _db.UserRoles on user.Id equals userRole.UserId
                                     join r in _db.Roles on userRole.RoleId equals r.Id
                                     where
                                     (
                                        (
                                            string.IsNullOrEmpty(search.Search.value) || user.Email.ToLower().Contains(search.Search.value.ToLower())
                                        )
                                        &&
                                        (string.IsNullOrEmpty(search.Role) || search.Role == r.Name)
                                        &&
                                        (search.Roles.Count == 0 || rolesName.Contains(r.Name))
                                        &&
                                        (r.Name != "SuperAdmin")
                                        &&
                                        (
                                        string.IsNullOrEmpty(search.Email) || user.Email.ToLower().Contains(search.Email.ToLower())
                                        )
                                    )
                                     select new UserDetailViewModel { Id = user.Id }
                            ).GroupBy(x => x.Id)
                            .Select(x => new UserDetailViewModel { Id = x.Max(m => m.Id) })
                            .AsQueryable();

                //var queryString = userQueryable.ToQueryString();

                var users = await userQueryable.Paginate(searchFilter);
                var filteredUserIds = users.Items.Select(x => x.Id);

                var userList = await _db.Users
                    .Where(x => filteredUserIds.Contains(x.Id))
                    .Select(x => new UserDetailViewModel
                    {
                        Id = x.Id,
                        Email = x.Email,
                        UserName = x.UserName,
                    }).ToListAsync();
                var roles = await _db.UserRoles
                  .Join(_db.Roles.Where(x => x.Name != "SuperAdmin"),
                              ur => ur.RoleId,
                              r => r.Id,
                              (ur, r) => new { UR = ur, R = r })
                  .Where(x => filteredUserIds.Contains(x.UR.UserId))
                  .Select(x => new UserRoleVM
                  {
                      RoleId = x.R.Id,
                      UserId = x.UR.UserId,
                      RoleName = x.R.Name
                  })
                  .ToListAsync();

                users.Items.ForEach(x =>
                {
                    x.Id = userList.Where(a => a.Id == x.Id).Select(x => x.Id).FirstOrDefault();
                    x.Email = userList.Where(a => a.Id == x.Id).Select(x => x.Email).FirstOrDefault();
                    x.UserName = userList.Where(a => a.Id == x.Id).Select(x => x.UserName).FirstOrDefault();
                    x.Roles = roles.Where(u => u.UserId == x.Id).Select(r => new UserRolesVM { Id = r.RoleId, Name = r.RoleName }).ToList();
                });
                var mappedUserList = _mapper.Map<List<T>>(users.Items);
                var paginatedModel = new PaginatedResultModel<T> { Items = mappedUserList, _links = users._links, _meta = users._meta };
                return paginatedModel;
            }
            catch (Exception ex)
            {
                _logger.LogError($"BeneficiaryService GetAll method threw an exception, Message: {ex.Message}");
                return new PaginatedResultModel<T>();
            }
        }

        public async Task<bool> SendNotification(MailRequestViewModel mailRequest)
        {
            try
            {
                NotificationViewModel viewModel = new()
                {
                    Message = JsonConvert.SerializeObject(mailRequest),
                    SendTo = mailRequest.SendTo,
                    Type = mailRequest.Type,
                };

                return (await _notificationService.Create(viewModel)).Status == System.Net.HttpStatusCode.OK;

            }
            catch (Exception ex)
            {
                _logger.LogError($"IdentityService SendMessage method threw an exception, Message: {ex.Message}");
                return false;
            }
        }
    }
}
