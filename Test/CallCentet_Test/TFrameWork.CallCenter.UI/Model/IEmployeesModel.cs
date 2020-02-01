using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFrameWork.CallCenter.UI.Model
{
    public interface IEmployeesModel
    {
        EmployeeModel[] GetEmployees();

        string GetFreeEmployee();

        event EventHandler<EventArgs> EmployeesLoadCompleted;

        void InitEmploees();
    }
}
