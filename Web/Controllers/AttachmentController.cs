using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.CommonServices;
using ViewModels.Shared;
using ViewModels.DataTable;
using Repositories.Services.AppSettingServices;
using Microsoft.AspNetCore.Mvc;
using ViewModels.Shared;
using Repositories.Shared.AttachmentService;
using ViewModels.CRUD;
using Centangle.Common.ResponseHelpers.Models;
using Pagination;
using ViewModels.Attachment;

namespace Web.Controllers
{
    [Authorize]

    public class AttachmentController : CrudBaseController<AttachmentVM, AttachmentVM, AttachmentVM, AttachmentVM, AttachmentSearchViewModel>
    {
        private readonly ILogger<AttachmentController> _logger;
        private readonly IMapper _mapper;
        private readonly IAttachmentService<AttachmentVM, AttachmentVM, AttachmentVM> _attachmentService;

        public AttachmentController
            (
                ILogger<AttachmentController> logger,
                IMapper mapper,
                IAttachmentService<AttachmentVM, AttachmentVM, AttachmentVM> attachmentService
            ) : base(attachmentService, logger, mapper, "Attachment", "Attachments")
        {
            _logger = logger;
            _mapper = mapper;
            _attachmentService = attachmentService;
        }

        [Route("Attachment/Index/{id:long}")]
        public async Task<IActionResult> Index(long id)
        {
            try
            {
                var attachments = await SearchAttachments(new AttachmentSearchViewModel { DisablePagination = true, Folder = new FolderBriefViewModel { Id = id } });
                var folder = new FolderDetailViewModel { Id = id, Attachments = attachments };
                return View(folder);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Attachment _GetAttachmentView method threw an exception, Message: {ex.Message}");
                return RedirectToAction("Error");
            }
        }

        [Route("Attachment/Create/{id:long}")]
        [HttpGet]
        public async Task<IActionResult> Create(long id)
        {
            return UpdateView(GetUpdateViewModel("Create", new AttachmentVM { Folder = new FolderBriefViewModel { Id = id } }));
        }

        [NonAction]
        public override ActionResult Create()
        {
            return base.Create();
        }
        [NonAction]
        public override ActionResult Index()
        {
            return View();
        }

        public async Task<List<AttachmentResponseVM>> SearchAttachments(AttachmentSearchViewModel search)
        {
            var response = await _attachmentService.GetAll<AttachmentVM>(search);
            var parsedResponse = response as RepositoryResponseWithModel<PaginatedResultModel<AttachmentVM>>;
            return _mapper.Map<List<AttachmentResponseVM>>(parsedResponse?.ReturnModel.Items) ?? new List<AttachmentResponseVM>();
        }

        public async Task<IActionResult> _GetFolders(AttachmentSearchViewModel search)
        {
            var response = await SearchAttachments(search);
            return View(response);
        }

        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {

            };
        }
    }
}
