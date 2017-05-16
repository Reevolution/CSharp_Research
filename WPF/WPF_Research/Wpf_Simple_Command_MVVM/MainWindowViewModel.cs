using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Wpf_Simple_Command_MVVM
{
    public class MainWindowViewModel
    {
        public BaseCommand SimpleAction { get; set; }

        public MainWindowViewModel()
        {
            SimpleAction = new BaseCommand(DoAction);
        }

        private void DoAction(object parameter)
        {
            MessageBox.Show("Do simple action", "Action", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
