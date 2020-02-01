using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TFrameWork.CallCenter.UI.CallCenterServiceServiceReference;

namespace TFrameWork.CallCenter.UI.Model
{
    public class EmployeesModel : IEmployeesModel
    {
        List<EmployeeModel> _employees = new List<EmployeeModel>();

        public event EventHandler<EventArgs> EmployeesLoadCompleted;

        ICallCenterService _callCenterServiceClient;

        public EmployeesModel() : this(new CallCenterServiceClient())
        {

        }

        public EmployeesModel(ICallCenterService callCenterServiceClient)
        {
            _callCenterServiceClient = callCenterServiceClient;
        }

        public EmployeeModel[] GetEmployees()
        {
            return _employees.ToArray();
        }

        public string GetFreeEmployee()
        {
            var employeeDto = _callCenterServiceClient.GetFreeEmloyee();

            if(employeeDto != null)
            {
                return employeeDto.Login;
            }

            return null;
        }

        public void InitEmploees()
        {
            var employeesDto = _callCenterServiceClient.GetEmployes();
            var employees = employeesDto.Select(n => Map(n));

            _employees.AddRange(employees);

            foreach (var employee in _employees)
            {
                employee.StateUpdated += Employee_StateUpdated;
            }

            EmployeesLoadCompleted?.Invoke(this, new EventArgs());
        }

        private void Employee_StateUpdated(object sender, StateUpdatedArgs e)
        {
            var login = e.Login;
            var status = Map(e.Status);

            _callCenterServiceClient.UpdateState(login, status);
        }

        private EmployeeModel Map(EmployeeDto employee)
        {
            return new EmployeeModel
            {
                Login = employee.Login,
                Active = false,
                Status = Map(employee.Status),
                EmployeeCategory = Map(employee.Category)
            };
        }

        private EmployeeStatus Map(Status status)
        {
            switch (status)
            {
                case Status.NotReady:
                    return EmployeeStatus.NotReady;

                case Status.InUse:
                    return EmployeeStatus.Busy;

                case Status.Free:
                    return EmployeeStatus.Free;

                default:
                    return EmployeeStatus.Busy;

            }
        }

        private Status Map(EmployeeStatus status)
        {
            switch (status)
            {
                case EmployeeStatus.NotReady:
                    return Status.NotReady;

                case EmployeeStatus.Busy:
                    return Status.InUse;

                case EmployeeStatus.Free:
                    return Status.Free;

                default:
                    return Status.InUse;

            }
        }

        private EmployeeCategory Map(CallCenterServiceServiceReference.EmployeeCategory category)
        {
            switch (category)
            {
                case CallCenterServiceServiceReference.EmployeeCategory.Operator:
                    return EmployeeCategory.Operator;

                case CallCenterServiceServiceReference.EmployeeCategory.Manager:
                    return EmployeeCategory.Manager;

                case CallCenterServiceServiceReference.EmployeeCategory.Director:
                    return EmployeeCategory.Director;

                default:
                    return EmployeeCategory.Operator;
            }
        }
    }
}
