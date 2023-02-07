using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Enums
{
    public enum TWRAlphabeticPartCatalog
    {
        [Display(Name = "A-Analyzer")]
        A = 1,
        [Display(Name = "B-Fin Fan")]
        B = 2,
        [Display(Name = "C-Vessel")]
        C = 3,
        [Display(Name = "CC-Chem Clean")]
        CC = 4



    }
}
