using AutoMapper;
using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Enums;
using Helpers.File;
using Helpers.Models.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using Models.AppSettings;
using Pagination;
using Repositories.Shared.AuthenticationService;
using ViewModels.Shared;

namespace Repositories.Shared.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IMapper _mapper;
        private readonly IFileHelper _fileHelper;
        public readonly ToranceContext _context;
        private readonly UserManager<ToranceUser> _userManager;
        private readonly IRepositoryResponse _response;
        private readonly ILogger<IdentityService> _logger;

        public AttachmentService(ToranceContext context, UserManager<ToranceUser> userManager,
            IRepositoryResponse response, ILogger<IdentityService> logger, IMapper mapper, IFileHelper fileHelper)
        {
            _context = context;
            _userManager = userManager;
            _response = response;
            _logger = logger;
            _mapper = mapper;
            _fileHelper = fileHelper;
        }

        public async Task<IRepositoryResponse> CreateMultiple(List<AttachmentVM> attachments)
        {
            try
            {
                await CreateAttachments(attachments);
                return new RepositoryResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Attachment threw the following exception");
            }

            return Response.BadRequestResponse(_response);
        }
        public async Task<IRepositoryResponse> CreateSingle(AttachmentVM attachment)
        {
            try
            {
                List<string> urls = new List<string>();
                _logger.LogDebug("Create Attachment method, number of Attachments: ");

                var imgName = DateTime.Now.Ticks;
                attachment.Url = _fileHelper.Save(attachment);

                var dbAttachments = _mapper.Map<MobileFile>(attachment);
                _logger.LogDebug("Create Attachmentt method, attachments mapped");

                await _context.AddAsync(dbAttachments);
                await _context.SaveChangesAsync();
                _logger.LogDebug("Create Attachmentt method, attachments saved");

                var response = new RepositoryResponseWithModel<long> { ReturnModel = dbAttachments.Id };
                return response;

                //return new RepositoryResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Attachment threw the following exception");
            }

            return Response.BadRequestResponse(_response);
        }

        public async Task<IRepositoryResponse> UpdateSingle(AttachmentVM attachment)
        {
            try
            {
                List<string> urls = new List<string>();
                _logger.LogDebug("UpdateSingle Attachment method, number of Attachments: ");

                var imgName = DateTime.Now.Ticks;
                attachment.Url = _fileHelper.Save(attachment);

                var dbAttachment = await _context.MobileFiles.Where(x => x.Id == attachment.Id && x.FileType == attachment.FileType).FirstOrDefaultAsync();

                //dbAttachment = _mapper.Map<MobileFile>(attachment);
                dbAttachment = _mapper.Map(attachment, dbAttachment);
                _logger.LogDebug("UpdateSingle Attachmentt method, attachments mapped");

                await _context.SaveChangesAsync();
                _logger.LogDebug("UpdateSingle Attachmentt method, attachments saved");

                return new RepositoryResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateSingle Attachment threw the following exception");
            }

            return Response.BadRequestResponse(_response);
        }

        //Update Attachment

        public async Task<IRepositoryResponse> Update(List<AttachmentVM> attachments, long id)
        {
            try
            {
                if (id > 0)
                {
                    var attachmentOld = await _context.MobileFiles.Where(x => x.Id == id).ToListAsync();

                    var attachmentsNew = _mapper.Map<List<MobileFile>>(attachments.Where(x => x.Id != null && x.Id > 0));

                    DeleteAttachment(attachmentsNew, attachmentOld);
                    await CreateAttachments(attachments.Where(x => (x.Id == null || x.Id == 0) && x.File != null).ToList());

                    var responseModel = new RepositoryResponseWithModel<List<AttachmentVM>>();
                    responseModel.ReturnModel = attachments;
                    return responseModel;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Attachment threw the following exception");
            }
            return Response.BadRequestResponse(_response);
        }

        private async Task CreateAttachments(List<AttachmentVM> attachments)
        {
            if (attachments.Count() > 0)
            {
                _logger.LogDebug("Create Attachment method, number of Attachments: ", attachments.Count());

                foreach (var attachment in attachments)
                {
                    attachment.Url = _fileHelper.Save(attachment);
                }

                var dbAttachments = _mapper.Map<List<Attachment>>(attachments);
                _logger.LogDebug("Create Attachmentt method, attachments mapped");

                await _context.AddRangeAsync(dbAttachments);
                await _context.SaveChangesAsync();
                _logger.LogDebug("Create Attachmentt method, attachments saved");
            }
        }

        private void DeleteAttachment(List<MobileFile> attachmentNew, List<MobileFile> attachmentOld)
        {
            var removedAttachments = attachmentOld.Except(attachmentNew, new IdComparer<MobileFile>());

            foreach (var attachment in removedAttachments)
            {
                attachment.IsDeleted = true;
            }
        }
        //public async Task<IRepositoryResponse> GetAll<T>(BaseSearchModel search)
        //{
        //    var data = search as AttachmentSearchVM;
        //    IQueryable<AttachmentListVM> attachmentQueryable = (from i in _context.Attachments
        //                                                        where data.EntityId <= 0 || i.EntityId.Equals(data.EntityId)
        //                                                        select new AttachmentListVM
        //                                                        {
        //                                                            Id = i.Id,
        //                                                            //  Name = i.Name,
        //                                                            Url = i.Url,
        //                                                            //   EntityId = i.EntityId,
        //                                                        }).AsQueryable();
        //    var result = attachmentQueryable as IQueryable<T>;

        //    var responseModel = new RepositoryResponseWithModel<PaginatedResultModel<T>>();
        //    responseModel.ReturnModel = await result.Paginate(search);
        //    return responseModel;
        //}

        public async Task<IRepositoryResponse> DeleteByEntity(long id, AttachmentEntityType entityType)
        {
            try
            {
                var attachmentQuery = _context.MobileFiles.Where(x => x.Id == id && x.FileType == entityType).Select(x => x.Id);
                var attachmentIds = await attachmentQuery.ToListAsync();
                if (await DeleteAttachment(attachmentIds))
                    return _response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"DeleteByParent threw the above exception");
            }
            return Response.BadRequestResponse(_response);
        }

        public async Task<IRepositoryResponse> Delete(List<long> attachmentIds)
        {
            try
            {
                if (await DeleteAttachment(attachmentIds))
                    return _response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete Attachment threw the following exception");
            }

            return Response.BadRequestResponse(_response);
        }

        private async Task<bool> DeleteAttachment(List<long> attachmentIds)
        {
            try
            {
                var attachments = await _context.MobileFiles.Where(x => attachmentIds.Contains(x.Id)).ToListAsync();
                attachments.ForEach(x => x.IsDeleted = true);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"DeleteAttachment is AttachmentService threw the above exception");
                return false;
            }
        }

        public Task<IRepositoryResponse> GetAll<M>(IBaseSearchModel model)
        {
            throw new NotImplementedException();
        }
    }

    public class IdComparer<T> : IEqualityComparer<T> where T : BaseDBModel
    {
        public bool Equals(T x, T y)
        {
            return x.Id == y.Id;
        }
        public int GetHashCode(T obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
