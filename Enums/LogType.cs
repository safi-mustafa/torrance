using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Enums
{
    public enum LogType
    {
        [Display(Name = "Time On Tools")]
        TimeOnTools,
        [Display(Name = "Welding Rod Record")]
        WeldingRodRecord
    }
}
