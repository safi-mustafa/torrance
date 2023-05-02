using Models.Common;
using Helpers.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enums;
using Models.Common.Interfaces;

namespace Models.WeldingRodRecord
{
    public class WRRLog : BaseDBModel, IApprove, IEmployeeId, IApproverId, IUnitId
    {
        public DateTime DateRodReturned { get; set; }
        public DateTime CalibrationDate { get; set; }
        public FumeControlUsedCatalog FumeControlUsed { get; set; }

        public string? WorkScope { get; set; }
        public string Twr { get; set; }
        public string? Email { get; set; }
        public DateTime RodCheckedOut { get; set; }
        public double RodCheckedOutLbs { get; set; }
        public double? RodReturnedWasteLbs { get; set; }
        public Status Status { get; set; }

        [ForeignKey("Department")]
        public long DepartmentId { get; set; }
        public Department Department { get; set; }

        [ForeignKey("Employee")]
        public long? EmployeeId { get; set; }
        public ToranceUser? Employee { get; set; }

        [ForeignKey("Unit")]
        public long UnitId { get; set; }
        public Unit Unit { get; set; }

        [ForeignKey("RodType")]
        public long RodTypeId { get; set; }
        public RodType RodType { get; set; }

        [ForeignKey("WeldMethod")]
        public long WeldMethodId { get; set; }
        public WeldMethod WeldMethod { get; set; }

        [ForeignKey("Location")]
        public long LocationId { get; set; }
        public Location Location { get; set; }

        [ForeignKey("Contractor")]
        public long? ContractorId { get; set; }
        public Contractor? Contractor { get; set; }

        [ForeignKey("Company")]
        public long CompanyId { get; set; }
        public Company Company { get; set; }

        [ForeignKey("Approver")]
        public long? ApproverId { get; set; }
        public ToranceUser? Approver { get; set; }
    }
}
