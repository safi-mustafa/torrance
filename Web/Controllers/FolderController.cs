using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Repositories.Services.FolderService;
using ViewModels.Shared;
using ViewModels.DataTable;
using Repositories.Services.AppSettingServices.MobileFileServices;
using Microsoft.AspNetCore.Mvc;
using ViewModels.Shared.Folder;
using Repositories.Shared.AttachmentService;
using ViewModels.CRUD;

namespace Web.Controllers
{
    [Authorize]

    public class FolderController : CrudBaseController<FolderModifyViewModel, FolderModifyViewModel, FolderDetailViewModel, FolderDetailViewModel, FolderSearchViewModel>
    {
        private readonly IFolderService<FolderModifyViewModel, FolderModifyViewModel, FolderDetailViewModel> _folderService;
        private readonly ILogger<FolderController> _logger;
        private readonly IAttachmentService<AttachmentVM, AttachmentVM, AttachmentVM> _attachmentService;

        public FolderController
            (
                IFolderService<FolderModifyViewModel, FolderModifyViewModel, FolderDetailViewModel> folderService, 
                ILogger<FolderController> logger, 
                IMapper mapper,
                IAttachmentService<AttachmentVM, AttachmentVM, AttachmentVM> attachmentService
            ) : base(folderService, logger, mapper, "Folder", "Folders")
        {
            _folderService = folderService;
            _logger = logger;
            _attachmentService = attachmentService;
        }

        public override List<DataTableViewModel> GetColumns()
        {
            return new List<DataTableViewModel>()
            {
                new DataTableViewModel{title = "Name",data = "Name"},
                new DataTableViewModel{title = "Status",data = "FormattedStatus"},
                new DataTableViewModel{title = "Action",data = null,className="text-right exclude-form-export"}

            };
        }
        public override ActionResult DataTableIndexView(CrudListViewModel vm)
        {
            return base.DataTableIndexView(vm);
            return View("~/Views/Folder/_Index.cshtml", vm);
        }

        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> _GetFolderView(FolderSearchViewModel search)
        {
            try
            {
                search.DisablePagination = true;
                var model = await _folderService.GetAll<FolderDetailViewModel>(search);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Folder _GetFolderView method threw an exception, Message: {ex.Message}");
            }
            return null;
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateAttachment(AttachmentVM model)
        {
            try
            {
                var folderId = await _attachmentService.Create(model);
                return RedirectToAction("_GetAttachmentView", new { id = model.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Folder CreateAttachment method threw an exception, Message: {ex.Message}");
            }
            return null;
        }

    }
}
