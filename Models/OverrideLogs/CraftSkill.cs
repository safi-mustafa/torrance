﻿using Helpers.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.OverrideLogs
{
    public class CraftSkill : BaseDBModel
    {
        public string Name { get; set; }

        public double Rate { get; set; }
    }
}
