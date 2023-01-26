using Select2.Model;
using System.ComponentModel;

namespace ViewModels.Shared
{
    public class FolderBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public FolderBriefViewModel() : base(true, "The Folder field is required.")
        {

        }
        [DisplayName("Folder")]
        public override string? Name { get; set; }
    }
}
