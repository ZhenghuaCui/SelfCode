using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NetTaste;
using Prism.Events;
using WpfApp3;
using WpfApp3.Common;
using WpfApp3.Data;
using WpfApp3.Events;
using WpfApp3.ViewModels;
using WpfApp3.Views;
using Wuhua.Main.Events;
using Wuhua.Main.ViewModels;

namespace Wuhua.Main.Views
{
    /// <summary>
    /// Interaction logic for OtherFuncControl.xaml
    /// </summary>
    public partial class OtherFuncControl : Window
    {
        MainWindowViewModel _mainVm;
        private MainWindow mainWindow;
        IEventAggregator _aggregator;

        public OtherFuncControl(MainWindow mainWindow, IEventAggregator aggregator)
        {
            _aggregator = aggregator;
            this.mainWindow = mainWindow;
            InitializeComponent();
        }

        private void CountBest_Click(object sender, RoutedEventArgs e)
        {
            if (!mainWindow.CheckTeamControl()) return;
            ChoosePage.Visibility = Visibility.Collapsed;
            MunuBorder.Visibility = Visibility.Visible;
            Type type = Type.GetType("Wuhua.Main.Views.CountWeaponControl");
            var win = (FrameworkElement)System.Activator.CreateInstance(type);
            this.transeperControl.Content = win;

        }
        public Task publishTask;
        private  void StartCount(object sender, RoutedEventArgs e)
        {
            UserControl control = (UserControl)transeperControl.Content;
            if (control.DataContext is CountWeaponControlViewModel)
            {
                var countWeaponVm = control.DataContext as CountWeaponControlViewModel;
                if(countWeaponVm.IncreInfos.Where(i => i.IsChecked).Count() < 3)
                {
                    MessageBox.Show("至少需要三个词条");
                    return;
                }
                App.Current.Dispatcher.Invoke(new Action(() => {
                    var window = new OutputControl(_aggregator);
                    window.Owner = this;
                    _aggregator.GetEvent<RoleSelectedEvent>()?.Publish(false);
                    _aggregator.GetEvent<CountWeaponEvent>()?.Publish(countWeaponVm.IncreInfos.Where(i => i.IsChecked).Select(i => i.IncreInfo).ToList());
                    var result = window.ShowDialog();
                }));
            }
            this.Close();
        }
    }
}
