﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Common.Validation
{
    public interface IValidateName
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
