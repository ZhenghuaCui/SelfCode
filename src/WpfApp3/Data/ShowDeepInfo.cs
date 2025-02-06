using System.Collections.ObjectModel;
using Prism.Mvvm;
using Wuhua.Model;

namespace WpfApp3.Data
{
    public class ShowDeepInfo : BindableBase
    {
        private ObservableCollection<ShowIncreInfo> deepIncres = new ObservableCollection<ShowIncreInfo>();
        public ObservableCollection<ShowIncreInfo> DeepIncres
        {
            get { return deepIncres; }
            set
            {
                SetProperty(ref deepIncres, value);
            }
        }

        private DeepInfo deepInfo;
        public DeepInfo DeepInfo
        {
            get { return deepInfo; }
            set
            {
                SetProperty(ref deepInfo, value);
            }
        }

    }
}
