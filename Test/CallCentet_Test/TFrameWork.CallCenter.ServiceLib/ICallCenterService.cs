using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFrameWork.CallCenter.ServiceLib
{
    public interface ICallCenterService
    {
        void Login(string login);

        void UpdateState(string login, Status status);

        EmployeeModel GetFreeEmloyee();

        EmployeeModel[] GetEmployes();
    }
}
