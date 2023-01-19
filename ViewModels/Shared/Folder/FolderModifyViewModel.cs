using Enums;
using Helpers.File;
using Microsoft.AspNetCore.Http;
using Models.Common.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ViewModels.Shared;

namespace ViewModels.Shared
{
    public class FolderModifyViewModel : BaseUpdateVM, IBaseCrudViewModel, IIdentitifier, IFileModel
    {
        [Required]
        [MaxLength(200)]
        [DisplayName("Name")]
        public string Name { get; set; }

        public string? IconUrl { get; set; }
       
        [DataType(DataType.Upload)]
        [MaxFileSize(25 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg" })]
        public IFormFile? File { get; set; }
        public string GetBaseFolder()
        {
            var ext = Path.GetExtension(File.FileName);
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
