using AduSkin.Controls.Metro;
using Prism.Events;
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
using WpfApp3.Data;
using WpfApp3.Events;
using WpfApp3.ViewModels;
using Wuhua.Model;

namespace WpfApp3.Views
{
	/// <summary>
	/// TeamDisplayControl.xaml 的交互逻辑
	/// </summary>
	public partial class TeamControl : UserControl
	{
		public TeamControl()
		{
			InitializeComponent();
		}

		private void DeleteItem_click(object sender, RoutedEventArgs e)
		{
			var obj = sender as AduFlatButton;
			var data = obj.DataContext as CustomShowWeapon;
		}
	}
}
