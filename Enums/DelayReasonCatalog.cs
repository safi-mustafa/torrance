using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
