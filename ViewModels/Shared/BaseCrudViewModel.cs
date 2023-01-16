using Enums;
using Helpers.Extensions;
using Models.Common.Interfaces;
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
}
