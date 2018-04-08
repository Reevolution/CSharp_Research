using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wpf_Simple_MVVM
{
    /// <summary>
    /// Interface INotifyPropertyChanged need for detect change from data.
    /// </summary>
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _data;

        public string Data
        {
            get { return _data; }
            set
            {
                _data = value;
                OnPropertyChanged(nameof(Data));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindowViewModel()
        {
            Data = "Simple Data";

            Thread thread = new Thread(ChangeText);
            thread.Start();
        }

        /// <summary>
        /// Simple test change data every time and check textbox.
        /// </summary>
        private void ChangeText()
        {
            for (int i = 0; i < 5; i++)
            {
                Data = $"Simple Data {i}";
                Thread.Sleep(250);
            }
        }
    }
}
