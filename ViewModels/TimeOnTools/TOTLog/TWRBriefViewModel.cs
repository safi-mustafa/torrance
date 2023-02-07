using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.TimeOnTools.TOTLog
{
    public class TWRBriefViewModel
    {
        public string ErrorMessage { get; } = "";
        public bool IsValidationEnabled { get; }

        public TWRBriefViewModel()
        {
            IsValidationEnabled = false;
            ErrorMessage = "";
        }
        public TWRBriefViewModel(bool isValidationEnabled)
        {
            IsValidationEnabled = isValidationEnabled;
            ErrorMessage = "";
        }
        public TWRBriefViewModel(bool isValidationEnabled, string errorMessage)
        {
            IsValidationEnabled = isValidationEnabled;
            ErrorMessage = errorMessage;
        }
        public string? Id { get; set; }
        public string? Name { get; set; }
    }
}
