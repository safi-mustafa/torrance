using Helpers.Models.Shared;
using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.TimeOnTools
{
    public class ShiftDelay :  BaseDBModel, IName
    {
        public string Name { get; set; }
    }
}
