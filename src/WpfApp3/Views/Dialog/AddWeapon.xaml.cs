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

namespace WpfApp3.Views
{
	/// <summary>
	/// AddWeapon.xaml 的交互逻辑
	/// </summary>
	public partial class AddWeapon : Window
	{
		public AddWeapon()
		{
			InitializeComponent();
		}

		private void OnClickCacel(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
		}

		private void OnClickOk(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}
	}
}
