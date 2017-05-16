using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Wpf_Simple_Command_MVVM
{
    /// <summary>
    /// Base command logic.
    /// </summary>
    public class BaseCommand : ICommand, INotifyPropertyChanged
    {
        private readonly Action<object> _command;

        public BaseCommand(Action<Object> command)
        {
            _command = command;
        }

        public event EventHandler CanExecuteChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _command.Invoke(parameter);
        }

        private bool _isEnableState;

        public bool IsEnabledState
        {
            get
            {
                return _isEnableState;
            }
            set
            {
                _isEnableState = value;
                OnPropertyChanged(nameof(IsEnabledState));
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
