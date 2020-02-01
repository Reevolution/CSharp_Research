using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFrameWork.CallCenter.ServiceLib
{
    public class EmployeeRepository : IEmployeeRepository
    {
        List<EmployeeModel> _employees = new List<EmployeeModel>()
        {
            new EmployeeModel() { Category = EmployeeCategory.Operator, Login = "petrov", Priority = 1, Status = Status.NotReady },
            new EmployeeModel() { Category = EmployeeCategory.Operator, Login = "ivanov", Priority = 1, Status = Status.NotReady },
            new EmployeeModel() { Category = EmployeeCategory.Operator, Login = "sidorov", Priority = 1, Status = Status.NotReady },
            new EmployeeModel() { Category = EmployeeCategory.Manager, Login = "kuznecov", Priority = 2, Status = Status.NotReady } ,
            new EmployeeModel() { Category = EmployeeCategory.Manager, Login = "milanov", Priority = 2, Status = Status.NotReady },
            new EmployeeModel() { Category = EmployeeCategory.Director, Login = "zivcov", Priority = 3, Status = Status.NotReady }
        };

        public EmployeeRepository()
        {
        }

        public EmployeeModel GetEmployee(string login)
        {
            var employee = _employees.FirstOrDefault(n => n.Login == login);

            return employee;
        }

        public EmployeeModel[] GetEmployees()
        {
            return _employees.ToArray();
        }
    }

    public interface IEmployeeRepository
    {
        EmployeeModel GetEmployee(string login);

        EmployeeModel[] GetEmployees();
    }
}
