using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Helpers.File;
using Helpers.Models.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.AppSettings;
using Pagination;
using Repositories.Common;
using System.Linq.Expressions;
using ViewModels.Shared;

namespace Repositories.Services.AppSettingServices.MobileFileServices
{
    public class MobileFileService : BaseService<MobileFile, BaseFileUpdateViewModel, BaseFileUpdateViewModel, BaseFileDetailViewModel>, IMobileFileService
    {
        private readonly ToranceContext _db;
        private readonly IMapper _mapper;
        private readonly IFileHelper _fileHelper;
        private readonly IRepositoryResponse _response;

        public MobileFileService(ToranceContext db, ILogger<MobileFileService> logger, IMapper mapper, IFileHelper fileHelper, IRepositoryResponse response) : base(db, logger, mapper, response)
        {
            _db = db;
            _mapper = mapper;
            _fileHelper = fileHelper;
            _response = response;
        }

        //public override Expression<Func<BadgeRoom, bool>> SetQueryFilter(IBaseSearchModel filters)
        //{
        //    var searchFilters = filters as BadgeRoomSearchViewModel;

        //    return x =>
        //                    (string.IsNullOrEmpty(searchFilters.Search.value) || x.Name.ToLower().Contains(searchFilters.Search.value.ToLower()))
        //                    &&
        //                    (string.IsNullOrEmpty(searchFilters.Name) || x.Name.ToLower().Contains(searchFilters.Name.ToLower()))
        //                    &&
        //                    (searchFilters.Status == null || x.ActiveStatus == searchFilters.Status)
        //                ;
        //}

        [HttpPost]
        public override Task<IRepositoryResponse> Create(BaseFileUpdateViewModel model)
        {
            model.Url = model.File != null ? _fileHelper.Save(model) : null;
            return base.Create(model);
        }

        public override Task<IRepositoryResponse> Update(BaseFileUpdateViewModel model)
        {
            model.Url = model.File != null ? _fileHelper.Save(model) : model.Url;
            return base.Update(model);
        }

    }
}
