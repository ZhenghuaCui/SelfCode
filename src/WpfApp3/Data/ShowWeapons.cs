using System.Collections.ObjectModel;
using Prism.Mvvm;
using Wuhua.Model;

namespace WpfApp3.Data
{
    public class ShowWeapons : BindableBase
    {
        public ShowWeapons(ObservableCollection<IncreInfo> increProList)
        {
            IncreProList = increProList;
        }
        public ObservableCollection<IncreInfo> IncreProList { get; private set; }
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

        private IncreInfo _selectedIncre = null;

        public IncreInfo SelectedIncre
        {
            get => _selectedIncre;
            set
            {
                _selectedIncre = value;
                SetProperty(ref _selectedIncre, value);
            }
        }
    }
}
