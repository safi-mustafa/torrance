﻿using Helpers.Models.Shared;
using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.OverrideLogs
{
    public class ReasonForRequest : BaseDBModel, IName
    {
        public string Name { get; set; }
    }
}
