﻿using AutoMapper;
using DataLibrary;
using Microsoft.Extensions.Logging;
using Repositories.Shared.AuthenticationService;
using Centangle.Common.ResponseHelpers.Models;
using Models.Common.Interfaces;
using ViewModels.Shared;
using Microsoft.AspNetCore.Identity;
using Models;
using Repositories.Services.CommonServices.UserService;
using Repositories.Shared.UserInfoServices;

namespace Repositories.Services.AppSettingServices.CompanyManagerService
{
    public class CompanyManagerService<CreateViewModel, UpdateViewModel, DetailViewModel> : UserService<CreateViewModel, UpdateViewModel, DetailViewModel>, ICompanyManagerService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly UserManager<ToranceUser> _userManager;
        private readonly ILogger<CompanyManagerService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identity;
        private readonly IRepositoryResponse _response;

        public CompanyManagerService(ToranceContext db, UserManager<ToranceUser> userManager, ILogger<CompanyManagerService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IMapper mapper, IIdentityService identity, IRepositoryResponse response, IUserInfoService userInfoService) : base(db, Enums.RolesCatalog.CompanyManager, userManager, logger, mapper, identity, response, userInfoService)
        {
            _db = db;
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
            _identity = identity;
            _response = response;
        }
    }
}
