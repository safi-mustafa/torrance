using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Enums
{
    public enum ActiveStatus
    {
        Active = 1,
        [Display(Name = "In-Active")]
        Inactive = 2
    }
}
