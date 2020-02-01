using System.ServiceModel;
using TFrameWork.CallCenter.ServiceLib;
using TFrameWork.CallCenter.WCF.Service.Dto;

namespace TFrameWork.CallCenter.WCF.Service
{
    [ServiceContract]
    public interface ICallCenterService
    {
        [OperationContract]
        void Login(string login);

        [OperationContract]
        void UpdateState(string login, Status status);

        [OperationContract]
        EmployeeDto GetFreeEmloyee();

        [OperationContract]
        EmployeeDto[] GetEmployes();
    }
}
