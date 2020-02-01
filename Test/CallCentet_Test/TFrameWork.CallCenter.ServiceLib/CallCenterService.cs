using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFrameWork.CallCenter.ServiceLib
{
    public class CallCenterService : ICallCenterService
    {
        readonly IEmployeeService _employeeService;
        readonly IEmployeeRepository _employeeRepository;

        public CallCenterService(IEmployeeService employeeService, IEmployeeRepository employeeRepository)
        {
            if (employeeService == null)
                throw new ArgumentNullException();

            if (employeeRepository == null)
                throw new ArgumentNullException();

            _employeeService = employeeService;
            _employeeRepository = employeeRepository;
        }

        public EmployeeModel[] GetEmployes()
        {
            var employees = _employeeRepository.GetEmployees();

            return employees;
        }

        public EmployeeModel GetFreeEmloyee()
        {
            var employee = _employeeService.GetFreeEmloyee();

            return employee;
        }

        public void Login(string login)
        {
            var employee = _employeeRepository.GetEmployee(login);

            if (employee == null)
                throw new InvalidOperationException();
        }

        public void UpdateState(string login, Status status)
        {
            _employeeService.UpdateState(login, status);
        }
    }
}
