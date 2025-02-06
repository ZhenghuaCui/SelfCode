using System.Collections.ObjectModel;
using Prism.Mvvm;
using WpfApp3.Common;
using Wuhua.Model;

namespace WpfApp3.Data
{
    public class ShowIncreInfo : BindableBase
    {
        public string Title { get; set; } = "";
        private string? increNum = "0";
        public string? IncreNum
        {
            get { return increNum; }
            set
            {
                int number;
                if (int.TryParse(value, out number))
                {
                    SetProperty(ref increNum, number.ToString());
                }
                else
                {
                    SetProperty(ref increNum, "0");
                }


            }
        }
        public ObservableCollection<IncreInfo> IncreInfoList { get; set; } = new ObservableCollection<IncreInfo>();

        private IncreInfo _selectedIncre = null;

        public IncreInfo SelectedIncre
        {
            get => _selectedIncre;
            set
            {
                SetProperty(ref _selectedIncre, value);
            }
        }
        private Occupation _selectedOccupation;
        public Occupation SelectedOccupation
        {
            get => _selectedOccupation;
            set
            {
                SetProperty(ref _selectedOccupation, value);
            }
        }

    }
}
