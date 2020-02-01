using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TFrameWork.CallCenter.ServiceLib;
using TFrameWork.CallCenter.WCF.Service.Dto;

using Lib = TFrameWork.CallCenter.ServiceLib;

namespace TFrameWork.CallCenter.WCF.Service
{
    public class CallCenterService : ICallCenterService
    {
        Lib.ICallCenterService _callCenterService;

        public CallCenterService(Lib.ICallCenterService callCenterService)
        {
            if (callCenterService == null)
                throw new ArgumentNullException();

            _callCenterService = callCenterService;
        }

        public EmployeeDto[] GetEmployes()
        {
            var employeeMpodels = _callCenterService.GetEmployes();

            var employees = employeeMpodels.Select(n => Map(n));

            return employees.ToArray();
        }

        public EmployeeDto GetFreeEmloyee()
        {
            var employeeModel = _callCenterService.GetFreeEmloyee();

            if(employeeModel != null)
            {
                return Map(employeeModel); 
            }

            return null;
        }

        public void Login(string login)
        {
            _callCenterService.Login(login);
        }

        public void UpdateState(string login, Status status)
        {
            _callCenterService.UpdateState(login, status);
        }

        EmployeeDto Map(Lib.EmployeeModel employee)
        {
            return new EmployeeDto()
            {
                Login = employee.Login,
                Category = employee.Category,
                Status = employee.Status,
                Priority = employee.Priority
            };
        }
    }
}
