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
                var alphabeticPartId = twrSplitted[2];
                var twrList = GetTWRAlphabeticList();
                var alphabeticText = twrList.Where(x => x.id == alphabeticPartId).Select(x => x.text).FirstOrDefault();
                AlphabeticPart = twrSplitted.Count() > 2 ? new Select2ViewModel
                {
                    id = twrSplitted[2],
                    text = alphabeticText
                } : new Select2ViewModel();
                Text = twrSplitted[3] ?? "";
            }
        }
        public string Name { get; set; } = "TWR";
        public Select2ViewModel NumericPart { get; set; } = new Select2ViewModel();
        public Select2ViewModel AlphabeticPart { get; set; } = new Select2ViewModel();
        public string Text { get; set; }

        public List<Select2ViewModel> GetTWRNumericList() => new List<Select2ViewModel>()
        {
            new Select2ViewModel
            {
                id = "01",
                text = "01"
            },
            new Select2ViewModel
            {
                id = "02",
                text = "02"
            },
            new Select2ViewModel
            {
                id = "03",
                text = "03"
            },
            new Select2ViewModel
            {
                id = "04",
                text = "04"
            },
            new Select2ViewModel
            {
                id = "05",
                text = "05"
            },
            new Select2ViewModel
            {
                id = "06",
                text = "06"
            },
            new Select2ViewModel
            {
                id = "07",
                text = "07"
            },
            new Select2ViewModel
            {
                id = "08",
                text = "08"
            },
            new Select2ViewModel
            {
                id = "09",
                text = "09"
            },
            new Select2ViewModel
            {
                id = "10",
                text = "10"
            },
            new Select2ViewModel
            {
                id = "12",
                text = "12"
            },
            new Select2ViewModel
            {
                id = "13",
                text = "13"
            },
            new Select2ViewModel
            {
                id = "17",
                text = "17"
            },
            new Select2ViewModel
            {
                id = "19",
                text = "19"
            },
            new Select2ViewModel
            {
                id = "20",
                text = "20"
            },
            new Select2ViewModel
            {
                id = "21",
                text = "21"
            },
            new Select2ViewModel
            {
                id = "22",
                text = "22"
            },
            new Select2ViewModel
            {
                id = "24",
                text = "24"
            },
            new Select2ViewModel
            {
                id = "25",
                text = "25"
            },
            new Select2ViewModel
            {
                id = "27",
                text = "27"
            },
            new Select2ViewModel
            {
                id = "28",
                text = "28"
            },
            new Select2ViewModel
            {
                id = "29",
                text = "29"
            },
            new Select2ViewModel
            {
                id = "30",
                text = "30"
            },
            new Select2ViewModel
            {
                id = "50",
                text = "50"
            },
            new Select2ViewModel
            {
                id = "51",
                text = "51"
            },
            new Select2ViewModel
            {
                id = "52",
                text = "52"
            },
            new Select2ViewModel
            {
                id = "53",
                text = "53"
            },
            new Select2ViewModel
            {
                id = "55",
                text = "55"
            },
            new Select2ViewModel
            {
                id = "56",
                text = "56"
            },
            new Select2ViewModel
            {
                id = "64",
                text = "64"
            },
            new Select2ViewModel
            {
                id = "65",
                text = "65"
            },
            new Select2ViewModel
            {
                id = "66",
                text = "66"
            },
            new Select2ViewModel
            {
                id = "67",
                text = "67"
            },
            new Select2ViewModel
            {
                id = "68",
                text = "68"
            },
            new Select2ViewModel
            {
                id = "72",
                text = "72"
            },
            new Select2ViewModel
            {
                id = "75",
                text = "75"
            },
            new Select2ViewModel
            {
                id = "76",
                text = "76"
            },
            new Select2ViewModel
            {
                id = "80",
                text = "80"
            },
            new Select2ViewModel
            {
                id = "81",
                text = "81"
            },
            new Select2ViewModel
            {
                id = "82",
                text = "82"
            },
            new Select2ViewModel
            {
                id = "83",
                text = "83"
            },
            new Select2ViewModel
            {
                id = "85",
                text = "85"
            },
            new Select2ViewModel
            {
                id = "98",
                text = "98"
            },
            new Select2ViewModel
            {
                id = "266",
                text = "266"
            }
        };

        public List<Select2ViewModel> GetTWRAlphabeticList() => new List<Select2ViewModel>() {
          new Select2ViewModel {
            text = "A-Analyzer",
            id = "A"
          },
          new Select2ViewModel {
            text = "B-Fin Fan",
            id = "B"
          },
          new Select2ViewModel {
            text = "C-Vessel",
            id = "C"
          },
          new Select2ViewModel {
            text = "CC-Chem Clean",
            id = "CC"
          },
          new Select2ViewModel {
            text = "D-Drum",
            id = "D"
          },
          new Select2ViewModel {
            text = "E-Exchanger",
            id = "E"
          },
          new Select2ViewModel {
            text = "Ei-Electrical",
            id = "Ei"
          },
          new Select2ViewModel {
            text = "F-Heater",
            id = "F"
          },
          new Select2ViewModel {
            text = "FC-Flow Controller",
            id = "FC"
          },
          new Select2ViewModel {
            text = "FE-Orifice Plate",
            id = "FE"
          },
          new Select2ViewModel {
            text = "FI-Flow Indicator",
            id = "FI"
          },
          new Select2ViewModel {
            text = "FO-Orifice Plate",
            id = "FO"
          },
          new Select2ViewModel {
            text = "FS-Flow Switch",
            id = "FS"
          },
          new Select2ViewModel {
            text = "FT-Flow Transmitter",
            id = "FT"
          },
          new Select2ViewModel {
            text = "FV-Control Valve",
            id = "FV"
          },
          new Select2ViewModel {
            text = "G-Pumps",
            id = "G"
          },
          new Select2ViewModel {
            text = "Gi-General Instrumentation",
            id = "Gi"
          },
          new Select2ViewModel {
            text = "HC-Control Valve",
            id = "HC"
          },
          new Select2ViewModel {
            text = "HV-Control Valve",
            id = "HV"
          },
          new Select2ViewModel {
            text = "J-Filters",
            id = "J"
          },
          new Select2ViewModel {
            text = "K-Compressor",
            id = "K"
          },
          new Select2ViewModel {
            text = "LC-Level Controller",
            id = "LC"
          },
          new Select2ViewModel {
            text = "LG-Level Glass",
            id = "LG"
          },
          new Select2ViewModel {
            text = "LI-Level Indicator",
            id = "LI"
          },
          new Select2ViewModel {
            text = "LS-Level Switch",
            id = "LS"
          },
          new Select2ViewModel {
            text = "LT-Level Transmitter",
            id = "LT"
          },
          new Select2ViewModel {
            text = "LV-Control Valve",
            id = "LV"
          },
          new Select2ViewModel {
            text = "M-Miscellaneous",
            id = "M"
          },
          new Select2ViewModel {
            text = "OR-Other Rotating",
            id = "OR"
          },
          new Select2ViewModel {
            text = "P-Piping",
            id = "P"
          },
          new Select2ViewModel {
            text = "PC-Pressure Controller",
            id = "PC"
          },
          new Select2ViewModel {
            text = "PCV-Control Valve",
            id = "PCV"
          },
          new Select2ViewModel {
            text = "PI-Pressure Indicator",
            id = "PI"
          },
          new Select2ViewModel {
            text = "PS-Pressure Switch",
            id = "PS"
          },
          new Select2ViewModel {
            text = "PT-Pressure Transmitter",
            id = "PT"
          },
          new Select2ViewModel {
            text = "PV-Control Valve",
            id = "PV"
          },
          new Select2ViewModel {
            text = "SD-Shut Down",
            id = "SD"
          },
          new Select2ViewModel {
            text = "SE-Specialty Equipment",
            id = "SE"
          },
          new Select2ViewModel {
            text = "SOL-Solenoid",
            id = "SOL"
          },
          new Select2ViewModel {
            text = "SU-Start Up",
            id = "SU"
          },
          new Select2ViewModel {
            text = "SV-Safety Valve",
            id = "SV"
          },
          new Select2ViewModel {
            text = "T-Tank",
            id = "T"
          },
          new Select2ViewModel {
            text = "TC-Temperature Controller",
            id = "TC"
          },
          new Select2ViewModel {
            text = "TCV-Control Valve",
            id = "TCV"
          },
          new Select2ViewModel {
            text = "TE-Temperature Element",
            id = "TE"
          },
          new Select2ViewModel {
            text = "TI-Temperature Indication",
            id = "TI"
          },
          new Select2ViewModel {
            text = "TS-Temperature Switch",
            id = "TS"
          },
          new Select2ViewModel {
            text = "TT-Temperature Transmitter",
            id = "TT"
          },
          new Select2ViewModel {
            text = "TV-Control Valve",
            id = "TV"
          },
          new Select2ViewModel {
            text = "V-Valve",
            id = "V"
          },
          new Select2ViewModel {
            text = "XV-Control Valve",
            id = "XV"
          }
        };
    }
}
