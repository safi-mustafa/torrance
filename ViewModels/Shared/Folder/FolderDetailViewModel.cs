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
        public List<AttachmentResponseVM> Attachments { get; set; }
    }

   
}
