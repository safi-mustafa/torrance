using Select2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Shared.Folder
{
    public class FolderViewModel : BaseCrudViewModel, ISelect2Data
    {
        public long? Id { get; set; }
        [DisplayName("Name")]
        public string Name { get; set; }
        public long AttachmentsCount
        {
            get
            {
                if (Attachments != null && Attachments.Count > 0)
                {
                    return Attachments.Count;
                }
                return 0;
            }
        }
        public List<AttachmentVM> Attachments { get; set; } = new List<AttachmentVM>();
    }
}
