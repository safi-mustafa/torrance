using System;
using Enums;
using Pagination;

namespace ViewModels.Shared
{
    public class FolderSearchViewModel : BaseSearchModel
    {
        public string Name { get; set; }
        public ActiveStatus? Status { get; set; }
    }
}

