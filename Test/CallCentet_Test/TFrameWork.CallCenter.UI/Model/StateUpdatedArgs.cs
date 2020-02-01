using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFrameWork.CallCenter.UI.Model
{
    public class StateUpdatedArgs : EventArgs
    {
        public string Login { get; }

        public EmployeeStatus Status { get; }

        public StateUpdatedArgs(string login, EmployeeStatus status)
        {
            Login = login;
            Status = status;
        }
    }
}
