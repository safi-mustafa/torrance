﻿using Enums;
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

        public AttachmentTypeCatalog AttachmentType { get; set; }
        public DateTime UploadDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public FolderBriefViewModel? Folder { get; set; } = new();
        public ActiveStatus ActiveStatus { get; set; }

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
        private List<string> _imgExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
        public AttachmentModifyViewModel()
        {

        }
        public AttachmentModifyViewModel(AttachmentEntityType fileType)
        {
            FileType = fileType;
        }
        [DataType(DataType.Upload)]
        [MaxFileSize(25 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".pptx", ".png", ".jpeg", ".pdf", ".docx", ".xlsx" })]
        public IFormFile? File { get; set; }
        public string? Url { get; set; } = "";
        public string PreviewImgUrl
        {
            get
            {
                var previewImgUrl = "";
                if (_imgExtensions.Contains(Type))
                {
                    return Url;
                }
                else if (Type == ".pdf")
                {
                    previewImgUrl = "/img/file-icons/pdf.png";
                }
                else if (Type == ".docx")
                {
                    previewImgUrl = "/img/file-icons/docx.png";
                }
                else if (Type == ".xlsx")
                {
                    previewImgUrl = "/img/file-icons/xlsx.png";
                }
                else if (Type == ".pptx")
                {
                    previewImgUrl = "/img/file-icons/pptx.png";
                }
                else
                {
                    previewImgUrl = "/img/file-icons/default.png";
                }
                return previewImgUrl;
            }
        }
        public string AttachmentTypeStr
        {
            get
            {
                //used in datatable library
                if (!_imgExtensions.Contains(Type))
                {
                    return "file";
                }
                return "image";
                
            }
        }
        private string? _type;
        public string Type { get => string.IsNullOrEmpty(_type) ? Path.GetExtension(File?.FileName) ?? "" : _type; set => _type = value; }
        public string? Name { get; set; }
        public AttachmentEntityType FileType { get; set; }
        public AttachmentTypeCatalog AttachmentType { get; set; }
        public DateTime UploadDate { get; set; }
        public FolderBriefViewModel? Folder { get; set; } = new();
        public ActiveStatus ActiveStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? EntityId { get; set; }
        public AttachmentEntityType EntityType { get; set; }

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

    public class ClipEmployeeModifyViewModel : IFileModel, IAttachmentUrl
    {
        [DataType(DataType.Upload)]
        [MaxFileSize(25 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".pptx", ".png", ".jpeg", ".pdf", ".docx", ".xlsx" })]
        public IFormFile? File { get; set; }
        public string? Url { get; set; } = "";
        private string? _type;
        public string Type { get => string.IsNullOrEmpty(_type) ? Path.GetExtension(File?.FileName) ?? "" : _type; set => _type = value; }
        public string? Name { get; set; }

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

        public AttachmentTypeCatalog AttachmentType { get; set; }
        public FolderBriefViewModel? Folder { get; set; } = new();
    }
}
