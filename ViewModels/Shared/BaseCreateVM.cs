﻿using Enums;
using Helpers.File;
using Microsoft.AspNetCore.Http;
using Models.Common.Interfaces;
using System.ComponentModel.DataAnnotations;
using ViewModels.Shared;

namespace ViewModels.Shared
{
    public class BaseCreateVM : BaseCrudViewModel
    {
    }

    public class BaseFileCreateViewModel : BaseCreateVM, IFileModel
    {
        public virtual AttachmentEntityType FileType { get; set; }
        [DataType(DataType.Upload)]
        [MaxFileSize(25 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".pptx", ".png", ".jpeg", ".pdf", ".docx", ".xlsx" })]
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
