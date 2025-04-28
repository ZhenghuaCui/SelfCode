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

namespace Wuhua.Main.Common.Components
{
    /// <summary>
    /// Interaction logic for OkCancelPannel.xaml
    /// </summary>
    public partial class OkCancelPannel : UserControl
    {
        public static readonly DependencyProperty OkCommandProperty=
            DependencyProperty.Register("OkCommand",typeof(ICommand),typeof(OkCancelPannel));
        public static readonly DependencyProperty OkCommandParameterProperty =
            DependencyProperty.Register("OkCommandParameter", typeof(object), typeof(OkCancelPannel));



        public static readonly DependencyProperty CancelCommandProperty =
            DependencyProperty.Register("CancelCommand", typeof(ICommand), typeof(OkCancelPannel));
        public static readonly DependencyProperty CancelCommandParameterProperty =
            DependencyProperty.Register("CancelCommandParameter", typeof(object), typeof(OkCancelPannel));

        public ICommand OkCommand
        {
            get
            {
                return (ICommand)GetValue(OkCommandProperty);
            }
            set { SetValue(OkCommandProperty, value); }
        }
        public object OkCommandParameter
        {
            get { return (object)GetValue(OkCommandParameterProperty); }
            set { SetValue(OkCommandParameterProperty,value); }
        }
        public ICommand CancelCommand
        {
            get
            {
                return (ICommand)GetValue(CancelCommandProperty);
            }
            set { SetValue(CancelCommandProperty, value); }
        }
        public object CancelCommandParameter
        {
            get { return (object)GetValue(CancelCommandParameterProperty); }
            set { SetValue(CancelCommandParameterProperty, value); }
        }

        public OkCancelPannel()
        {
            InitializeComponent();
        }

        private void Ok_OnClick(object sender, RoutedEventArgs e)
        {
            if (OkCommand != null && OkCommand.CanExecute(OkCommandParameter))
            {
                OkCommand.Execute(OkCommandParameter);
            }
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            if (CancelCommand != null && CancelCommand.CanExecute(CancelCommandParameter))
            {
                CancelCommand.Execute(CancelCommandParameter);
            }
        }
    }
}
