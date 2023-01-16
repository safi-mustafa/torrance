﻿using Enums;
using Helpers.Models.Shared;
using Models.Common;
using Helpers.Models.Shared;
using Models.WeldingRodRecord;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.TimeOnTools
{
    public class TOTLog : BaseDBModel
    {
        public DateTime Date { get; set; }
        public string Twr { get; set; }
        public long ManHours { get; set; }
        public DateTime StartOfWork { get; set; }
        public string DelayReason { get; set; }
        public string JobDescription { get; set; }
        public long ManPower { get; set; }
        public long EquipmentNo { get; set; }
        public double HoursDelayed { get; set; }
        public ActiveStatus Status { get; set; }

        [ForeignKey("Department")]
        public long DepartmentId { get; set; }
        public Department Department { get; set; }

        [ForeignKey("Unit")]
        public long UnitId { get; set; }
        public Unit Unit { get; set; }

        [ForeignKey("Contractor")]
        public long ContractorId { get; set; }
        public Contractor Contractor { get; set; }

        [ForeignKey("ShiftDelay")]
        public long ShiftDelayId { get; set; }
        public ShiftDelay ShiftDelay { get; set; }

        [ForeignKey("ReworkDelay")]
        public long ReworkDelayId { get; set; }
        public ReworkDelay ReworkDelay { get; set; }

        [ForeignKey("PermitType")]
        public long PermitTypeId { get; set; }
        public PermitType PermitType { get; set; }

        [ForeignKey("Shift")]
        public long ShiftId { get; set; }
        public Shift Shift { get; set; }

        [ForeignKey("Approver")]
        public long ApproverId { get; set; }
        public ToranceUser Approver { get; set; }

        [ForeignKey("Foreman")]
        public long ForemanId { get; set; }
        public ToranceUser Foreman { get; set; }
    }
}
