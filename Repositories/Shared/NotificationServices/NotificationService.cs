using System.Linq.Expressions;
using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Enums;
using Microsoft.Extensions.Logging;
using Models;
using Models.Common;
using Models.Common.Interfaces;
using Pagination;
using Repositories.Common;
using ViewModels.Notification;
using ViewModels.Shared;

namespace Repositories.Shared.NotificationServices
{
    public class NotificationService<CreateViewModel, UpdateViewModel, DetailViewModel> : BaseService<Notification, CreateViewModel, UpdateViewModel, DetailViewModel>, INotificationService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger<NotificationService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryResponse _response;

        public NotificationService(ToranceContext db, ILogger<NotificationService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IMapper mapper, IRepositoryResponse response) : base(db, logger, mapper, response)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _response = response;
        }

        public override Expression<Func<Notification, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as NotificationSearchViewModel;

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.Type.ToString().ToLower().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (searchFilters.Type == null || x.Type == searchFilters.Type)
                            &&
                            (searchFilters.IsSent == null || x.IsSent == searchFilters.IsSent)
                        ;
        }
    }
}
