using System;
using System.ComponentModel.DataAnnotations;

namespace Enums
{
    public enum DuringExecutionCatalog
    {
        [Display(Name = "Approved - In Scope")]
        Approved,
        [Display(Name = "Change In Scope")]
        Change,
        Rejected
    }
}

