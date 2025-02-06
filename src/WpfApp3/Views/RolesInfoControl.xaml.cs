using Prism.Ioc;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp3.ViewModels;

namespace WpfApp3.Views
{
	/// <summary>
	/// RolesInfoControl.xaml 的交互逻辑
	/// </summary>
	public partial class RolesInfoControl : UserControl
	{
		public RolesInfoControl()
		{
			InitializeComponent();
			var a = Application.Current as App;
			var ContainerRegistry = (IContainerRegistry)a.Container;
			var vm = this.DataContext as RolesInfoControlViewModel;
			ContainerRegistry.RegisterInstance(vm, "RolesInfo");
		}
	}
}
