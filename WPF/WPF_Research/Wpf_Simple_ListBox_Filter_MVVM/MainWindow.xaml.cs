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

namespace Wpf_Simple_ListBox_Filter_MVVM
{
    // GitHub repo.
    // https://github.com/Reevolution/CSharp_Research

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Simple Example binding listBox and filter him by word.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();

            // Init event when list box ready, or listBox return null.
            this.listBox.Loaded += new RoutedEventHandler(ListBoxLoaded);
        }

        private void ListBoxLoaded(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listBox.ItemsSource);
            view.Filter = Filter;
        }

        private bool Filter(object item)
        {
            // Simple filter by word.
            if (String.IsNullOrEmpty(textBox.Text))
                return true;
            else
                return (item).ToString().IndexOf(textBox.Text, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void SearchTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (CollectionViewSource.GetDefaultView(listBox.ItemsSource) != null)
                CollectionViewSource.GetDefaultView(listBox.ItemsSource).Refresh();
        }
    }
}
