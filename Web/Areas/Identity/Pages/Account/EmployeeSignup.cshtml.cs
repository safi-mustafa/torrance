// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using DataLibrary;
using Microsoft.EntityFrameworkCore;
using Repositories.Services.CommonServices.UserService;
using Helpers.Extensions;
using System.Security.Claims;
using Enums;
using Centangle.Common.ResponseHelpers.Models;
using System.Net;
using ViewModels.Authentication.User;
using ViewModels.WeldingRodRecord.Employee;
using Repositories.Services.AppSettingServices.EmployeeService;
using AutoMapper;
using System.ComponentModel;
using ViewModels.Common.Company;

namespace Web.Areas.Identity.Pages.Account
{
    public class EmployeeSignupModel : PageModel
    {
        private readonly SignInManager<ToranceUser> _signInManager;
        private readonly ToranceContext _db;
        private readonly ILogger<LoginModel> _logger;
        private readonly UserManager<ToranceUser> _userManager;
        private readonly IEmployeeService<EmployeeModifyViewModel, EmployeeModifyViewModel, EmployeeDetailViewModel> _employeeService;
        private readonly IMapper _mapper;

        public EmployeeSignupModel(SignInManager<ToranceUser> signInManager,
            ToranceContext db,
            ILogger<LoginModel> logger,
            UserManager<ToranceUser> userManager,
            IEmployeeService<EmployeeModifyViewModel, EmployeeModifyViewModel, EmployeeDetailViewModel> employeeService,
            IMapper mapper)
        {
            _signInManager = signInManager;
            _db = db;
            _logger = logger;
            _userManager = userManager;
            _employeeService = employeeService;
            _mapper = mapper;
        }



        [BindProperty]
        public InputModel Input { get; set; }


        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            public long Id { get; set; }
            public CompanyBriefViewModel Company { get; set; } = new CompanyBriefViewModel(true, "The field Company is required.");

            [Required]
            [Display(Name = "Full Name")]
            public string FullName { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [DisplayName("Access Code")]
            [RegularExpression(@"^(\d{4})$", ErrorMessage = "Access Code must be of 4-digits.")]
            [Remote(action: "ValidateAccessCode", controller: "User", AdditionalFields = "Id,AccessCode", ErrorMessage = "Access Code already in use.")]
            public string AccessCode { get; set; }

            [DisplayName("Confirm Access Code")]
            [RegularExpression(@"^(\d{4})$", ErrorMessage = "Confirm Access Code must be of 4-digits.")]
            [Compare("AccessCode", ErrorMessage = "The Access Code and Confirm Access Code do not match.")]
            public string ConfirmAccessCode { get; set; }

        }



        public async Task OnGetAsync()
        {

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var model = new EmployeeModifyViewModel
                {
                    Company = Input.Company,
                    FullName = Input.FullName,
                    Email = Input.Email,
                    AccessCode = Input.AccessCode,
                };
                bool isUnique = await _employeeService.IsAccessCodeUnique(0, model.AccessCode);
                bool isEmailUnique = await _employeeService.IsEmailUnique(0, model.Email);
                if (!isUnique)
                {
                    ModelState.AddModelError("Input.AccessCode", "Access Code already in use.");
                }
                else if (!isEmailUnique)
                {
                    ModelState.AddModelError("Input.Email", "Email already in use.");
                }
                else
                {
                    model.Password = "TorrancePass";
                    model.ChangePassword = false;
                    model.ActiveStatus = Enums.ActiveStatus.Active;
                    var data = await _employeeService.Create(model);
                    if (data.Status == HttpStatusCode.OK)
                    {
                        var parsedResponse = data as RepositoryResponseWithModel<long>;
                        var id = parsedResponse?.ReturnModel ?? 0;
                        var message = "Account created successfully. Please login using pin code.";
                        return RedirectToPage("/Account/Login", new { area = "Identity", successMessage = message });

                    }
                    return Page();
                }
            }
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
