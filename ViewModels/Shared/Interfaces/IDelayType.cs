using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.TimeOnTools;

namespace ViewModels.Shared.Interfaces
{
    public interface IDelayType
    {
        public DelayTypeBriefViewModel DelayType { get; set; } 
    }
}
