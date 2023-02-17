using Select2.Model;
using System.ComponentModel;

namespace ViewModels.AppSettings.Administrator
{
    public class AdministratorBriefViewModel : BaseBriefVM, ISelect2Data
    {
        public AdministratorBriefViewModel() : base(true, "The Administrator field is required.")
        {

        }

        public AdministratorBriefViewModel(bool isValidationEnabled) : base(isValidationEnabled, "The Administrator field is required.")
        {

        }
        [DisplayName("Administrator")]
        public override string? Name { get; set; }

    }

}
