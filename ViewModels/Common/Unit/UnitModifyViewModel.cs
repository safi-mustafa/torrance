using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Models.Common.Interfaces;
using ViewModels.Shared;
using Enums;

namespace ViewModels.Common.Unit
{
    public class UnitModifyViewModel : BaseUpdateVM, IBaseCrudViewModel, IIdentitifier
    {
        [Required]
        [MaxLength(200)]
        [DisplayName("Name")]
        public string Name { get; set; }
        [DisplayName("Cost Tracker Unit")]
        public string CostTrackerUnit { get; set; }
        public LogType? Type { get; set; }
    }
}
