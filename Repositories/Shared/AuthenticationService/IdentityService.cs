using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Models;
using ViewModels.Authentication;

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
        
        public IdentityService(
            UserManager<ToranceUser> userManager,
            IUserStore<ToranceUser> userStore,
            SignInManager<ToranceUser> signInManager,
            ILogger<IdentityService> logger,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<long> CreateUser(SignUpModel model, IDbContextTransaction transaction, string optionalUsernamePrefix = "")
        {
            try
            {
                var user = _mapper.Map<ToranceUser>(model);
                try
                {
                    var result = await _userManager.CreateAsync(user, model.EmployeeId);
                    var role = model.Role != null ? model.Role : "Admin";
                    if (result.Succeeded)
                    {
                        if ((await _userManager.AddToRoleAsync(user, role)).Succeeded)
                        {
                            return user.Id;
                        }
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "CraeteUser threw the following exception");
                    await transaction.RollbackAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CraeteUser threw the following exception");
                await transaction.RollbackAsync();
            }
            return -1;
        }


        public async Task<bool> UpdateUser(SignUpModel model, IDbContextTransaction transaction)
        {
            string errorMsg = "There was some problem in updating data. Please try again later.";
            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId.ToString());
                if (user != null)
                {
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.PhoneNumber = model.PhoneNumber;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        await _userManager.RemovePasswordAsync(user);
                        var response = await _userManager.AddPasswordAsync(user, model.EmployeeId);
                        if (response.Succeeded)
                        {
                            return result.Succeeded;
                        }
                        else
                        {
                            _logger.LogWarning("Error! Password not updating.");
                            await transaction.RollbackAsync();
                        }
                    }
                    else
                    {
                        _logger.LogWarning(errorMsg, "Warning while updating user");
                        await transaction.RollbackAsync();
                    }
                }
                else
                {
                    await transaction.RollbackAsync();
                    return false;

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateUser threw following exception.");
            }
            await transaction.RollbackAsync();
            return false;
        }
    }
}
