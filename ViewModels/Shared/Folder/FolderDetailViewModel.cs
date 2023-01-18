using ViewModels.Shared;
using Pagination;
using Select2.Model;
using Enums;
using System.ComponentModel;

namespace ViewModels.Shared
{
    public class FolderDetailViewModel : BaseCrudViewModel, ISelect2Data
    {
        public long Id { get; set; }
        [DisplayName("Name")]
        public string Name { get; set; }
        public string? IconUrl { get; set; }
        public long AttachmentsCount
        {
            get
            {
                if(Attachments != null && Attachments.Count > 0)
                {
                    return Attachments.Count;
                }
                return 0;
            }
        }
        public List<AttachmentVM> Attachments { get; set; } = new List<AttachmentVM>();

    }

    public class FolderViewModel
    {
        public List<FolderDetailViewModel> Folders { get; set; } = new List<FolderDetailViewModel>();
        public long EmployeeId { get; set; }
    }



    public class FolderSearchViewModel : BaseSearchModel
    {
        public string? Name { get; set; }
        public ActiveStatus? Status { get; set; }
        public long? EmployeeId { get; set; }
    }
}
