using System;
using System.ComponentModel.DataAnnotations;

namespace Enums
{
    public enum NotificationEntityType
    {
        [Display(Name = "Time on Tools Log")]
        TOTLog,
        [Display(Name = "Welding Rod Record Log")]
        WRRLog,
        [Display(Name = "Override Log")]
        OverrideLog

    }
}

