using Enums;
using Helpers.Extensions;
using Models.Common.Interfaces;
using Pagination;
using System.ComponentModel;

namespace ViewModels.Shared
{
    public interface IBaseCrudViewModel : IIsActive
    {

    }
    public class BaseCrudViewModel : IBaseCrudViewModel
    {
        [DisplayName("Status")]
        public ActiveStatus ActiveStatus { get; set; } = ActiveStatus.Active;
        public string FormattedStatus
        {
            get
            {
                return ActiveStatus.GetDisplayName();
            }
        }
    }

    public class BaseFileDetailViewModel : BaseCrudViewModel
    {
        public long Id { get; set; }
        public string? Url { get; set; }
        public virtual AttachmentEntityType FileType { get; set; }
    }

    public class BaseFileSearchViewModel : BaseSearchModel
    {
        public virtual AttachmentEntityType FileType { get; set; }
    }
}
