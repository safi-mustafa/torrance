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
    [Authorize(Roles = "SuperAdmin,Approver")]
    public class AttachmentController : CrudBaseController<AttachmentModifyViewModel, AttachmentModifyViewModel, AttachmentModifyViewModel, AttachmentModifyViewModel, AttachmentSearchViewModel>
    {
        private readonly ILogger<AttachmentController> _logger;
        private readonly IMapper _mapper;
        private readonly IAttachmentService<AttachmentModifyViewModel, AttachmentModifyViewModel, AttachmentModifyViewModel> _attachmentService;

        public AttachmentController
            (
                ILogger<AttachmentController> logger,
                IMapper mapper,
                IAttachmentService<AttachmentModifyViewModel, AttachmentModifyViewModel, AttachmentModifyViewModel> attachmentService
            ) : base(attachmentService, logger, mapper, "Attachment", "Attachments", false, false)
        {
            _logger = logger;
            _mapper = mapper;
            _attachmentService = attachmentService;
        }

        //[Route("Attachment/Index/{id:long}/{name:string}")]
        [HttpGet]
        public async Task<IActionResult> Index(long id, string name)
        {
            try
            {
                var attachments = await SearchAttachments(new AttachmentSearchViewModel { DisablePagination = true, Folder = new FolderBriefViewModel { Id = id } });
                var folder = new FolderDetailViewModel { Id = id, Name = name, Attachments = attachments };
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
            return UpdateView(GetUpdateViewModel("Create", new AttachmentModifyViewModel { Folder = new FolderBriefViewModel { Id = id } }));
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

        public override Task<ActionResult> Create(AttachmentModifyViewModel model)
        {
            ValidateAttachmentViewModel(model);
            return base.Create(model);
        }

        public override Task<ActionResult> Update(AttachmentModifyViewModel model)
        {
            ValidateAttachmentViewModel(model);
            return base.Update(model);
        }

        public async Task<List<AttachmentResponseVM>> SearchAttachments(AttachmentSearchViewModel search)
        {
            var response = await _attachmentService.GetAll<AttachmentModifyViewModel>(search);
            var parsedResponse = response as RepositoryResponseWithModel<PaginatedResultModel<AttachmentModifyViewModel>>;
            return _mapper.Map<List<AttachmentResponseVM>>(parsedResponse?.ReturnModel.Items) ?? new List<AttachmentResponseVM>();
        }

        [HttpPost]
        public async Task<IActionResult> _GetAttachments(AttachmentSearchViewModel search)
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


        private void ValidateAttachmentViewModel(AttachmentModifyViewModel model)
        {
            if (model.AttachmentType == Enums.AttachmentTypeCatalog.File)
            {
                model.Url = null;
            }
            else
            {
                model.File = null;
                model.Type = "link";
            }
            if (model.AttachmentType == Enums.AttachmentTypeCatalog.File && model.Id < 1 && model.File == null)
            {
                ModelState.AddModelError("File", "The field File is required.");
            }
            else if (model.AttachmentType == Enums.AttachmentTypeCatalog.Link && string.IsNullOrEmpty(model.Url))
            {
                ModelState.AddModelError("Url", "The field Link is required.");
            }
        }
    }
}
