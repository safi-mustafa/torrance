﻿using Select2.Model;
using System.ComponentModel;
using ViewModels.Shared;

namespace ViewModels
{
    public class FCOReasonDetailViewModel : BaseCrudViewModel, ISelect2Data
    {
        public long? Id { get; set; }
        [DisplayName("Name")]
        public string Name { get; set; }
    }
}