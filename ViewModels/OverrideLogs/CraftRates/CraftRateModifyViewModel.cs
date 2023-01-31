﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Models.Common.Interfaces;
using ViewModels.Shared;

namespace ViewModels.OverrideLogs
{
    public class CraftRateModifyViewModel : BaseUpdateVM, IBaseCrudViewModel, IIdentitifier
    {
        [Required]
        [MaxLength(200)]
        [DisplayName("Rate")]
        public float Rate { get; set; }
    }
}
