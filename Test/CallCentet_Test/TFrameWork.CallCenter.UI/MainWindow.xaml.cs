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

using TFrameWork.CallCenter.UI.Model;

namespace TFrameWork.CallCenter.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly MainWindowViewModel _mainWindowViewModel;

        public MainWindow()
        {
            _mainWindowViewModel = new MainWindowViewModel();

            InitializeComponent();

            this.DataContext = _mainWindowViewModel;
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            _mainWindowViewModel.AddCustomer();
        }

        private void Remove_Button_Click(object sender, RoutedEventArgs e)
        {
            _mainWindowViewModel.RemoveCustomer();
        }
    }
}
