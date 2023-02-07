using Enums;
using Helpers.Extensions;
using Select2.Model;

namespace ViewModels.TimeOnTools.TOTLog
{
    public class TWRViewModel
    {
        public TWRViewModel()
        {
        }
        public TWRViewModel(string twr)
        {
            var twrSplitted = twr.Split('-');
            if (twrSplitted.Count() > 0)
            {
                Name = twrSplitted[0] ?? "";
                NumericPart = twrSplitted.Count() > 2 ? new Select2ViewModel
                {
                    id = twrSplitted[1],
                    text =twrSplitted[1]
                } : new Select2ViewModel();
                AlphabeticPart = twrSplitted.Count() > 2 ? new Select2ViewModel
                {
                    id = twrSplitted[2],
                    text = twrSplitted[2]
                } : new Select2ViewModel();
                Text = twrSplitted[3] ?? "";
            }
        }
        public string Name { get; set; } = "TWR";
        public Select2ViewModel NumericPart { get; set; } = new Select2ViewModel();
        public Select2ViewModel AlphabeticPart { get; set; } = new Select2ViewModel();
        public string Text { get; set; }
    }
}
