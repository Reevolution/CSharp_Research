using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

namespace Wpf_Simple_ListBox_Scroll_MVVM
{
    // GitHub repo.
    // https://github.com/Reevolution/CSharp_Research

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Simple Example auto scroll list box to down in MVVM pattern.
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel viewModel = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();

            DataContext = viewModel;
            viewModel.Items.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChangedMethod);
        }

        private void CollectionChangedMethod(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (checkBox.IsChecked == null || !checkBox.IsChecked == false) return;
            Dispatcher.BeginInvoke((Action)(() =>
            {
                if (listBox == null) throw new ArgumentNullException("ListBox", "ListBox cannot be null");
                if (!listBox.IsInitialized) throw new InvalidOperationException("ListBox is in an invalid state: IsInitialized == false");
                if (listBox.Items.Count == 0) return;

                listBox.ScrollIntoView(listBox.Items[listBox.Items.Count - 1]);
            }));
        }
    }
}
