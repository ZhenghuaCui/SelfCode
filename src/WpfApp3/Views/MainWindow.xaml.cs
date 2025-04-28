using Prism.Events;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using WpfApp3.Common;
using WpfApp3.Data;
using WpfApp3.Events;
using WpfApp3.ViewModels;
using WpfApp3.Views;
using Wuhua.Main.Data;
using Wuhua.Main.Events;
using Wuhua.Main.Views;
using Wuhua.Model;

namespace WpfApp3.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		IEventAggregator _aggregator;
		private MainWindowViewModel vm;
		public MainWindow(IContainerProvider containerProvider, IEventAggregator aggregator)
		{
			_aggregator = aggregator;
			vm = new MainWindowViewModel(containerProvider, aggregator);
			this.DataContext = vm;
            InitializeComponent();
		}


        private void NewRole_Click(object sender, RoutedEventArgs e)
		{

			var window = new AddRoles();
			window.Owner = this;
			_aggregator.GetEvent<ManageRolesEvent>()?.Publish(false);
			var result = window.ShowDialog();
		}

		private void NewWeapon_Click(object sender, RoutedEventArgs e)
		{
			var window = new AddWeapon();
			window.Owner = this;
			var result = window.ShowDialog();
		}
		public bool CheckTeamControl()
		{
            var teamVm = teamControl.DataContext as TeamControlViewModel;
            bool isReady = false;
            foreach (TeamDisplay display in teamVm.TeamList)
            {
                if (display.IsChecked && display.SelectedRole != null && display.SelectedWeapon != null)
                {
                    if (display.SelectedRole.RoleType.Equals((int)RoleTypeEnum.Worker))
                    {
                        MessageBox.Show("请选择输出类角色");
                        return false;
                    }
                    isReady = true;
                }

            }
            if (!isReady)
            {
                MessageBox.Show("请正确选择角色及武器");
                return false;
            }
			return true;
        }
		private void ReadyCount_Click(object sender, RoutedEventArgs e)
		{
            if (!CheckTeamControl()) return;
            var window = new OutputControl(_aggregator);
			window.Owner = this;
			_aggregator.GetEvent<RoleSelectedEvent>()?.Publish(true);
			var result = window.ShowDialog();
		}

		private void EditRole_Click(object sender, RoutedEventArgs e)
		{
			var window = new AddRoles();
			window.Owner = this;
			_aggregator.GetEvent<ManageRolesEvent>()?.Publish(true);
			var result = window.ShowDialog();
		}


		private void ClearReault_Click(object sender, RoutedEventArgs e)
		{
			vm.ResultList.Clear();
		}

		private void NewDeep_Click(object sender, RoutedEventArgs e)
		{
			var window = new DeepControl();
			window.Owner = this;
			var result = window.ShowDialog();
		}

		private void NewLearn_Click(object sender, RoutedEventArgs e)
		{
			var window = new LearnControl();
			window.Owner = this;
			var result = window.ShowDialog();
		}

        private void On_Closed(object sender, System.EventArgs e)
        {
            var teamVm = teamControl.DataContext as TeamControlViewModel;
            teamVm.Dispose();
            vm.Dispose();
        }

        private void OtherFunc_Click(object sender, RoutedEventArgs e)
        {
            var window = new OtherFuncControl(this, _aggregator);
            window.Owner = this;
            var result = window.ShowDialog();
        }
    }
}
