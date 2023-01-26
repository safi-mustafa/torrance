using Enums;
using Helpers.File;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.Shared
{
    public class AttachmentVM : IFileModel
    {
        public long? Id { get; set; }
        [DataType(DataType.Upload)]
        [MaxFileSize(25 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".pptx", ".png", ".jpeg", ".pdf", ".docx", ".xlsx" })]
        public IFormFile File { get; set; }
        public string Url { get; set; }
        public string ExtensionType { get; set; }
        public string Name { get; set; }
        public FolderDetailViewModel F { get; set; }
       public AttachmentEntityType FileType { get; set; }
        public DateTime UploadDate { get; set; }
        public string GetBaseFolder()
        {
            var ext = ExtensionType;
            if (ext == ".jpg" || ext == ".jpeg" || ext == ".png")
            {
                return "Images";
            }
            if (ext == ".mp4")
            {
                return "Videos";
            }
            if (ext == ".pdf" || ext == ".docx" || ext == ".xlsx" || ext == ".txt")
            {
                return "Documents";
            }
            return "Others";
        }
    }
}
