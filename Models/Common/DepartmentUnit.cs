using System;
using Helpers.Models.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Common
{
    public class DepartmentUnit : BaseDBModel
    {
        [ForeignKey("Department")]
        public long DepartmentId { get; set; }
        public Department Department { get; set; }

        [ForeignKey("Unit")]
        public long UnitId { get; set; }
        public Unit Unit { get; set; }
    }
}

