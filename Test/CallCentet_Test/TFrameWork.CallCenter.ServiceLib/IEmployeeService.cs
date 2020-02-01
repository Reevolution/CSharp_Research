using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFrameWork.CallCenter.ServiceLib
{
    public interface IEmployeeService
    {
        EmployeeModel GetFreeEmloyee();
        void UpdateState(string login, Status status);
    }


}
