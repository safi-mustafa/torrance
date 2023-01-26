using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Models.Common.Interfaces
{
    public interface IFolderAttachment
    {
        string? IconUrl { get; set; }
        IFormFile? File { get; set; }
    }
}

