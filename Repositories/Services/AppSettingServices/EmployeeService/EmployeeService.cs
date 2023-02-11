using AutoMapper;
using DataLibrary;
using Microsoft.Extensions.Logging;
using Repositories.Shared.AuthenticationService;
using Centangle.Common.ResponseHelpers.Models;
using Models.Common.Interfaces;
using ViewModels.Shared;
using Microsoft.AspNetCore.Identity;
using Models;
using Repositories.Services.CommonServices.UserService;

namespace Repositories.Services.AppSettingServices.EmployeeService
{
    public class EmployeeService<CreateViewModel, UpdateViewModel, DetailViewModel> : UserService<CreateViewModel, UpdateViewModel, DetailViewModel>, IEmployeeService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly UserManager<ToranceUser> _userManager;
        private readonly ILogger<EmployeeService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identity;
        private readonly IRepositoryResponse _response;

        public EmployeeService(ToranceContext db, UserManager<ToranceUser> userManager, ILogger<EmployeeService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IMapper mapper, IIdentityService identity, IRepositoryResponse response) : base(db, Enums.RolesCatalog.Employee, userManager, logger, mapper, identity, response)
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
