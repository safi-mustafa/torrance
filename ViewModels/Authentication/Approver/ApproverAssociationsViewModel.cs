using Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ViewModels.Authentication.User;
using ViewModels.Common.Department;
using ViewModels.Common.Unit;

namespace ViewModels.Authentication.Approver
{
    public class ApproverAssociationsViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Department")]
        public DepartmentBriefViewModel Department { get; set; } = new DepartmentBriefViewModel();

        [Display(Name = "Unit")]
        public UnitBriefViewModel Unit { get; set; } = new UnitBriefViewModel(false);

        
    }

    public class ApproverAssociationNotificationViewModel : ApproverAssociationsViewModel
    {
        public ApproverBriefViewModel Approver { get; set; } = new(false);
    }
}
