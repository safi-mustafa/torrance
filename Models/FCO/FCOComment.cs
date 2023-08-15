using System;
using System.ComponentModel.DataAnnotations.Schema;
using Helpers.Models.Shared;

namespace Models.FCO
{
    public class FCOComment : BaseDBModel
    {
        public string Comment { get; set; }

        [ForeignKey("FCOLog")]
        public long FCOLogId { get; set; }
        public FCOLog FCOLog { get; set; }
    }
}

