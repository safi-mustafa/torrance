using AutoMapper;
using DataLibrary;
using Helpers.Extensions;
using Microsoft.Extensions.Logging;
using Pagination;
using Repositories.Common;
using Repositories.Services.TimeOnToolServices.ShiftService;
using System.Linq.Expressions;
using ViewModels.Authentication;
using ViewModels.Authentication.Approver;
using ViewModels.TomeOnTools.SOW;

namespace Repositories.Services.TimeOnToolServices.UserService
{
    public class UserService : IUserService
    {
        private readonly ToranceContext _db;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;

        public UserService(ToranceContext db, ILogger<UserService> logger, IMapper mapper) 
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PaginatedResultModel<UserBriefViewModel>> GetUsers(UserSearchViewModel model)
        {
            var response = (from u in _db.Users
                            join ur in _db.UserRoles on u.Id equals ur.UserId
                            join r in _db.Roles on ur.RoleId equals r.Id
                            where
                            (
                                (r.Name == model.Type)
                                &&
                                (string.IsNullOrEmpty(model.Email) || u.Email.ToLower().Contains(model.Email.ToLower()))
                            )
                            select new UserBriefViewModel
                            {
                                Id = u.Id,
                                Name = u.Email
                            }).AsQueryable();
            return await response.Paginate(model);
        }
    }
}
