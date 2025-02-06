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
using System.Windows.Shapes;
using WpfApp3.Events;
using WpfApp3.ViewModels;

namespace WpfApp3.Views
{
	/// <summary>
	/// OutputControl.xaml 的交互逻辑
	/// </summary>
	public partial class OutputControl : Window
	{
		private IEventAggregator _aggregator;
		public OutputControl(IEventAggregator aggregator)
		{
			InitializeComponent();
			_aggregator = aggregator;
		}

		private void StartCount(object sender, RoutedEventArgs e)
		{
			_aggregator.GetEvent<CountDamageStartEvent>()?.Publish(true);
		}

		private void NextStep(object sender, RoutedEventArgs e)
		{
			rolesInfoControl.Visibility = Visibility.Collapsed;
			damageControl.Visibility = Visibility.Visible;
			nextButton.Visibility = Visibility.Collapsed;
			startButton.Visibility = Visibility.Visible;
		}
		private void DisposeWindow()
		{
			var vmRole = rolesInfoControl.DataContext as RolesInfoControlViewModel;
			vmRole.Dispose();
            var vmDamage = damageControl.DataContext as DamageOutControlViewModel;
            vmDamage.Dispose();

        }
		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			DisposeWindow();
            this.Close();
		}

        private void On_OutputWindowClosed(object sender, EventArgs e)
        {
            DisposeWindow();
        }
    }
}
