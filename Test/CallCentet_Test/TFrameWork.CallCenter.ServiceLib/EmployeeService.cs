using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using System.Timers;

namespace TFrameWork.CallCenter.ServiceLib
{
    public class EmployeeService : IEmployeeService
    {
        readonly IEmployeeSettings _employeeSettings;
        readonly IEmployeeRepository _employeeRepository;

        Dictionary<string, EmployeeEntry> _employees = new Dictionary<string, EmployeeEntry>();

        public EmployeeService(IEmployeeSettings employeeSettings, IEmployeeRepository employeeRepository)
        {
            if (employeeSettings == null)
                throw new ArgumentNullException();

            if (employeeRepository == null)
                throw new ArgumentNullException();

            _employeeSettings = employeeSettings;
            _employeeRepository = employeeRepository;
        }

        public EmployeeModel GetFreeEmloyee()
        {
            var employeeEntries = _employees
                .Where(n => n.Value.Employee.Status == Status.Free)
                .OrderBy(n=> n.Value.Employee.Priority)
                .Select(n=> n.Value);

            var entry = employeeEntries.FirstOrDefault();

            if(entry!= null)
            {

                return entry.Employee;
            }

            return null;
        }

        public void UpdateState(string login, Status status)
        {
            EmployeeEntry employeeEntry;
            if (_employees.TryGetValue(login, out employeeEntry))
            {
                employeeEntry.Employee.Status = status;

                _employees[login] = new EmployeeEntry(employeeEntry.Employee, new Timer(_employeeSettings.TimeOut));
            }
            else
            {
                var employee = _employeeRepository.GetEmployee(login);
                employee.Status = status;

                _employees[login] = new EmployeeEntry(employee, new Timer(_employeeSettings.TimeOut));
            }
        }

        class EmployeeEntry
        {
            readonly Timer _timeOut;

            public EmployeeModel Employee { get; }

            public EmployeeEntry(EmployeeModel employee, Timer timeOut)
            {
                if (employee == null)
                    throw new ArgumentNullException();

                if (timeOut == null)
                    throw new ArgumentNullException();

                Employee = employee;
                _timeOut = timeOut;

                _timeOut.Elapsed += TimeOut_Elapsed;

                _timeOut.Start();
            }

            private void TimeOut_Elapsed(object sender, ElapsedEventArgs e)
            {
                Employee.Status = Status.NotReady;
                _timeOut.Stop();
            }
        }
    }
}
