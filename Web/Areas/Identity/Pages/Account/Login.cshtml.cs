// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Models;
using DataLibrary;
using Helpers.Extensions;
using Microsoft.EntityFrameworkCore;
using Enums;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Security.Claims;

namespace Web.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ToranceUser> _signInManager;
        private readonly UserManager<ToranceUser> _userManager;
        private readonly ToranceContext _db;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<ToranceUser> signInManager, UserManager<ToranceUser> userManager, ToranceContext db, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _db = db;
            _logger = logger;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = await _userManager.Users.Include(u => u.Company).SingleOrDefaultAsync(u => u.Email == Input.Email && u.IsDeleted==false);
                if (user != null)
                {
                    if (await _userManager.IsInRoleAsync(user, RolesCatalog.Employee.ToString()))
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return Page();
                    }

                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                    var result = await _userManager.CheckPasswordAsync(user, Input.Password);
                    if (result)
                    {

                        if (user.ChangePassword)
                        {
                            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                            return RedirectToPage("./ResetPassword", new { Code = code, Email = user.Email, IsFirstTimeLogin = true });
                        }
                        else
                        {
                            var url = await GetReturnUrl(returnUrl, user);
                            //if (await _userManager.IsInRoleAsync(user, RolesCatalog.Employee.ToString()))
                            //{
                            //    await _userManager.AddClaimAsync(user, new Claim("CompanyId", user.Company.Id.ToString()));
                            //    await _userManager.AddClaimAsync(user, new Claim("CompanyName", user.Company.Name.ToString()));
                            //}
                            var signInResult = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                            if (signInResult.RequiresTwoFactor)
                            {
                                return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                            }
                            if (signInResult.IsLockedOut)
                            {
                                _logger.LogWarning("User account locked out.");
                                return RedirectToPage("./Lockout");
                            }
                            return LocalRedirect(url);
                        }

                    }
                    else
                    {
                        var encodedAccessCode = Input.Password.EncodePasswordToBase64();
                        var userWithAccessCode = await _db.Users.Where(x => x.Email == Input.Email && x.AccessCode == encodedAccessCode).FirstOrDefaultAsync();
                        if (userWithAccessCode != null)
                        {
                            await _signInManager.SignInAsync(userWithAccessCode, true);
                            var url = await GetReturnUrl(returnUrl, user);
                            return LocalRedirect(url);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            return Page();
                        }

                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private async Task<string> GetReturnUrl(string returnUrl, ToranceUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var role = userRoles.First();

            if (role == RolesCatalog.Approver.ToString())
            {
                return "/Approval";
            }
            else if (role == RolesCatalog.Employee.ToString())
            {
                return "/TOTLog";
            }
            else if (role == RolesCatalog.CompanyManager.ToString())
            {
                return "/OverrideLog";
            }
            _logger.LogInformation("User logged in.");
            return returnUrl;
        }
    }
}
