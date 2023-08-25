using System;
using System.ComponentModel.DataAnnotations;
using Enums;

namespace ViewModels.FCO.FCOLog
{
    public class FCOLogApproveViewModel
    {
        public FCOLogApproveViewModel()
        {
        }
        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "Id is Required.")]
        public long Id { get; set; }
        [Required]
        public Status Status { get; set; }
        public bool IsUnauthenticatedApproval { get; set; }
        public long? ApproverId { get; set; }
        public Guid? NotificationId { get; set; }
        public string? Comment { get; set; }
        [Display(Name = "Approver Type")]
        public ApproverType? ApproverType { get; set; }
    }
}

