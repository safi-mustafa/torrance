using Enums;
using System.ComponentModel;

namespace ViewModels.Shared
{
    public class BaseUpdateVM : BaseCrudViewModel
    {
        public long Id { get; set; }

        [DisplayName("Status")]
        public ActiveStatus ActiveStatus { get; set; } = ActiveStatus.Active;
        public bool IsCreated { get => Id <= 0; }
    }
}
