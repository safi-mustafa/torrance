using AutoMapper;
using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Enums;
using Helpers.File;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.AppSettings;
using Models.Common.Interfaces;
using Pagination;
using Repositories.Common;
using System.Linq.Expressions;
using ViewModels.AppSettings.MobileFiles.Dropbox;
using ViewModels.Shared;

namespace Repositories.Services.AppSettingServices.DropboxServices
{
    public class DropboxService<CreateViewModel, UpdateViewModel, DetailViewModel> : BaseService<Dropbox, CreateViewModel, UpdateViewModel, DetailViewModel>, IDropboxService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger<DropboxService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;
        private readonly IFileHelper _fileHelper;
        private readonly IRepositoryResponse _response;

        public DropboxService
            (
            ToranceContext db, 
            ILogger<DropboxService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, 
            IMapper mapper, 
            IFileHelper fileHelper, 
            IRepositoryResponse response
            ) : base(db, logger, mapper, response)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _fileHelper = fileHelper;
            _response = response;
        }

        public override Expression<Func<Dropbox, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as DropboxSearchViewModel;
            return x => (
                            (searchFilters.ActiveStatus == null || x.ActiveStatus == searchFilters.ActiveStatus)
                        )
            ;
        }

        public async Task<IRepositoryResponse> UpdateLinkStatus()
        {
            try
            {
                var dropboxList = await _db.Dropboxes.ToListAsync();
                if (dropboxList.Count > 0)
                {
                    dropboxList.ForEach(x => x.ActiveStatus = ActiveStatus.Inactive);
                    await _db.SaveChangesAsync();
                    return _response;
                }
                _logger.LogWarning($"No records found.");

                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"UpdateLinkStatus() threw the following exception");
                return Response.BadRequestResponse(_response);
            }
        }
    }
}
