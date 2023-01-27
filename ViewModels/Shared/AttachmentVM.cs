using Enums;
using Helpers.File;
using Microsoft.AspNetCore.Http;
using Models.Common;
using Models.Common.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.Shared
{
    public class AttachmentVM : BaseCreateVM, IFileModel, IAttachmentUrl, IBaseCrudViewModel
    {
        [DataType(DataType.Upload)]
        [MaxFileSize(25 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".pptx", ".png", ".jpeg", ".pdf", ".docx", ".xlsx" })]
        public IFormFile File { get; set; }
        public string? Url { get; set; } = "";
        private string? _type;
        public string Type { get => string.IsNullOrEmpty(_type) ? Path.GetExtension(File.FileName) : _type; set => _type = value; }
        public string Name { get; set; }
        public AttachmentEntityType FileType { get; set; }
        public DateTime UploadDate { get; set; }
        public FolderBriefViewModel? Folder { get; set; } = new();
        public ActiveStatus ActiveStatus { get; set; }
        public DateTime CreatedOn { get; set; }

        public string GetBaseFolder()
        {
            var ext = Type;
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

    public class AttachmentModifyViewModel : BaseUpdateVM, IFileModel, IAttachmentUrl, IBaseCrudViewModel, IIdentitifier
    {
        [DataType(DataType.Upload)]
        [MaxFileSize(25 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".pptx", ".png", ".jpeg", ".pdf", ".docx", ".xlsx" })]
        public IFormFile? File { get; set; }
        public string? Url { get; set; } = "";
        private string? _type;
        public string Type { get => string.IsNullOrEmpty(_type) ? Path.GetExtension(File?.FileName) ?? "" : _type; set => _type = value; }
        public string Name { get; set; }
        public AttachmentEntityType FileType { get; set; }
        public DateTime UploadDate { get; set; }
        public FolderBriefViewModel? Folder { get; set; } = new();
        public ActiveStatus ActiveStatus { get; set; }
        public DateTime CreatedOn { get; set; }

        public string GetBaseFolder()
        {
            var ext = Type;
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

    public class AttachmentResponseVM
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public DateTime CreatedOn { get; set; }
        public FolderBriefViewModel? Folder { get; set; } = new();
    }
}
