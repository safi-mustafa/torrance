using Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using ViewModels.TimeOnTools.ReworkDelay;
using ViewModels.TimeOnTools.ShiftDelay;
using ViewModels.TimeOnTools.StartOfWorkDelay;

namespace ViewModels.Shared
{
    public class LogDelayReasonCreateVM: BaseCreateVM
    {
        public ShiftDelayBriefViewModel ShiftDelay { get; set; } = new ShiftDelayBriefViewModel();
        [Required]
        public DelayReasonCatalog DelayReason { get; set; }
        public StartOfWorkDelayBriefViewModel StartOfWorkDelay { get; set; } = new StartOfWorkDelayBriefViewModel();

        public ReworkDelayBriefViewModel ReworkDelay { get; set; } = new ReworkDelayBriefViewModel();
        public void Validate(ModelStateDictionary modelState)
        {
            if (DelayReason == DelayReasonCatalog.ReworkDelay && (ReworkDelay == null || ReworkDelay.Id == null || ReworkDelay?.Id < 1))
            {
                modelState.AddModelError("ReworkDelay", "The field Rework Delay is required");
            }
            else if (DelayReason == DelayReasonCatalog.ShiftDelay && (ShiftDelay == null || ShiftDelay.Id == null || ShiftDelay?.Id < 1))
            {
                modelState.AddModelError("ShiftDelay", "The field Shift Delay is required");
            }
            else if (DelayReason == DelayReasonCatalog.StartOfWork && (StartOfWorkDelay == null || StartOfWorkDelay.Id == null || StartOfWorkDelay.Id < 1))
            {
                modelState.AddModelError("StartOfWorkDelay", "The field Start Of Work is required.");
            }
        }
    }
}
