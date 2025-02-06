using Prism.Ioc;
using Prism.Regions;
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
	/// IncreInjuryControl.xaml 的交互逻辑
	/// </summary>
	public partial class IncreInjuryControl : UserControl
	{
		public IncreInjuryControl()
		{
			var vm = new DamageBonusViewModel();
			this.DataContext = vm;
			InitializeComponent();
			var a = Application.Current as App;
			var ContainerRegistry = (IContainerRegistry)a.Container;
			ContainerRegistry.RegisterInstance(vm, "IncreInjury");
		}
	}
}
