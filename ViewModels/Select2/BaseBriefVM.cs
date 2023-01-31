using Helper.Attributes;
using System.ComponentModel;

namespace ViewModels
{

    public interface IBaseBriefVM
    {
        long Id { get; set; }
        string? Name { get; set; }
    }
    public class BaseBriefVM : IBaseBriefVM
    {
        public string ErrorMessage { get; } = "";
        public bool IsValidationEnabled { get; }

        public BaseBriefVM()
        {
            IsValidationEnabled = false;
            ErrorMessage = "";
        }
        public BaseBriefVM(bool isValidationEnabled)
        {
            IsValidationEnabled = isValidationEnabled;
            ErrorMessage = "";
        }
        public BaseBriefVM(bool isValidationEnabled, string errorMessage)
        {
            IsValidationEnabled = isValidationEnabled;
            ErrorMessage = errorMessage;
        }
        [RequiredSelect2("ErrorMessage", "IsValidationEnabled")]
        public virtual long Id { get; set; }
        public virtual string? Name { get; set; }
    }

    public class BaseImageBriefVM : BaseBriefVM
    {
        public string ImageUrl { get; set; }
    }


    public class InputBriefVM
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

}
