using Enums;
using Helpers.File;
using Microsoft.AspNetCore.Http;
using Models.Common.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.Shared
{
    public class BaseUpdateVM : BaseCrudViewModel
    {
        public long Id { get; set; }
        public bool IsCreated { get => Id <= 0; }
    }
}
