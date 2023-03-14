using Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using ViewModels.Shared.Interfaces;
using ViewModels.TimeOnTools;
using ViewModels.TimeOnTools.ReworkDelay;
using ViewModels.TimeOnTools.ShiftDelay;
using ViewModels.TimeOnTools.StartOfWorkDelay;
namespace ViewModels.Shared
{
    public class LogDelayReasonUpdateVM : BaseUpdateVM, IDelayType
    {
        public ShiftDelayBriefViewModel ShiftDelay { get; set; } = new ShiftDelayBriefViewModel();
        [Required]
        [Display(Name = "Delay Type")]
        public DelayTypeBriefViewModel DelayType { get; set; } = new DelayTypeBriefViewModel();
        public StartOfWorkDelayBriefViewModel StartOfWorkDelay { get; set; } = new StartOfWorkDelayBriefViewModel();

        public ReworkDelayBriefViewModel ReworkDelay { get; set; } = new ReworkDelayBriefViewModel();
        public void Validate(ModelStateDictionary modelState)
        {
            modelState.Remove("ShiftDelay");
            modelState.Remove("ReworkDelay");
            modelState.Remove("StartOfWorkDelay");
            modelState.Remove("ShiftDelay.Id");
            modelState.Remove("ReworkDelay.Id");
            modelState.Remove("StartOfWorkDelay.Id");
            if (DelayType.Identifier == DelayReasonCatalog.ReworkDelay.ToString())
            {
                if ((ReworkDelay == null || ReworkDelay.Id == null || ReworkDelay?.Id < 1))
                {
                    modelState.AddModelError("ReworkDelay.Id", "The field Rework Delay is required");
                }

            }
            else if (DelayType.Identifier == DelayReasonCatalog.ShiftDelay.ToString())
            {
                if (ShiftDelay == null || ShiftDelay.Id == null || ShiftDelay?.Id < 1)
                {
                    modelState.AddModelError("ShiftDelay.Id", "The field Shift Delay is required");
                }

            }
            else if (DelayType.Identifier == DelayReasonCatalog.StartOfWork.ToString())
            {
                if ((StartOfWorkDelay == null || StartOfWorkDelay.Id == null || StartOfWorkDelay.Id < 1))
                {
                    modelState.AddModelError("StartOfWorkDelay.Id", "The field Start Of Work is required.");
                }

            }
        }
    }
}
