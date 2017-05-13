using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Wpf_Simple_ListBox_Scroll_MVVM
{
    public class MainWindowViewModel
    {
        public ObservableCollection<SimpleData> Items { get; set; }

        public MainWindowViewModel()
        {
            Items = new ObservableCollection<SimpleData>();

            Thread thread = new Thread(AddItems);
            thread.Start();
        }

        private void AddItems()
        {
            for (int i = 0; i < 25; i++)
            {
                // Send in main thread for not crush application.
                Application.Current.Dispatcher.Invoke(
                    () =>
                    {
                        Items.Add(new SimpleData() { Id = i, Data = $"This is simple data {i}" });
                    });

                Thread.Sleep(250);
            }
        }
    }
}
