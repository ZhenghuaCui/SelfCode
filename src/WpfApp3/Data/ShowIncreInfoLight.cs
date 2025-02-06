using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Wuhua.Model;

namespace Wuhua.Main.Data
{
    public class ShowIncreInfoLight : BindableBase
    {
        private bool isChecked = false;
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                SetProperty(ref isChecked, value);
            }
        }
        private string increNum = "0";
        public string IncreNum
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

        private IncreInfo increInfo;
        public IncreInfo IncreInfo
        {
            get { return increInfo; }
            set
            {
                SetProperty(ref increInfo, value);
            }
        }
    }
}
