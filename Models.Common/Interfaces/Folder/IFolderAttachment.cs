using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Models.Common.Interfaces
{
    public interface IFolderIcon
    {
        string? IconUrl { get; set; }
    }
}

