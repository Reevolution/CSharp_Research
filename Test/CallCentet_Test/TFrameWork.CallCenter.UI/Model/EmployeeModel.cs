using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace TFrameWork.CallCenter.UI.Model
{
    public class EmployeeModel : INotifyPropertyChanged
    {
        Timer _timeOut = new Timer(5000);

        public EmployeeCategory EmployeeCategory { get; set; }

        private EmployeeStatus _status;

        public EmployeeStatus Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;

                    OnStateUpdated(Login, _status);
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        public string Login { get; set; }

        private bool _active;
        public bool Active
        {
            get { return _active; }
            set
            {
                if (_active != value)
                {
                    _active = value;

                    Status = _active ? EmployeeStatus.Free : EmployeeStatus.NotReady;

                    if (_active)
                    {
                        _timeOut.Start();
                    }
                    else
                    {
                        _timeOut.Stop();
                    }

                    OnStateUpdated(Login, _active ? EmployeeStatus.Free : EmployeeStatus.NotReady);
                    OnPropertyChanged(nameof(Active));
                }
            }
        }

        public event EventHandler<StateUpdatedArgs> StateUpdated;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected virtual void OnStateUpdated(string login, EmployeeStatus status)
        {
            StateUpdated.Invoke(this, new StateUpdatedArgs(login, status));
        }

        public EmployeeModel()
        {
            _timeOut.Elapsed += _timeOut_Elapsed;
        }

        private void _timeOut_Elapsed(object sender, ElapsedEventArgs e)
        {
            StateUpdated.Invoke(this, new StateUpdatedArgs(Login, Status));
        }
    }
}
