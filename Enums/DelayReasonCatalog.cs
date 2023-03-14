using System.ComponentModel.DataAnnotations;

namespace Enums
{
    public enum DelayReasonCatalog
    {
        [Display(Name = "Start Of Work")]
        StartOfWork,
        [Display(Name = "Shift Delay")]
        ShiftDelay,
        [Display(Name = "Rework Delay")]
        ReworkDelay
    }
}
