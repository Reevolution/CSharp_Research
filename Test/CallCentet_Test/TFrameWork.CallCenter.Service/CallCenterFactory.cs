using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Unity;
using Unity.Lifetime;

using TFrameWork.CallCenter.ServiceLib;

using Lib = TFrameWork.CallCenter.ServiceLib;

namespace TFrameWork.CallCenter.WCF.Service
{
    public class CallCenterFactory : DependencyInjectionServiceHostFactory
    {
        public void RegisterDependencies(IUnityContainer container)
        {
            container
                
                .RegisterType<ICallCenterService, CallCenterService>()
                .RegisterType<Lib.ICallCenterService, Lib.CallCenterService>()
                .RegisterType<IEmployeeService, EmployeeService>(new SingletonLifetimeManager())
                .RegisterType<IEmployeeRepository, EmployeeRepository>()
                .RegisterType<IEmployeeSettings, XmlEmployeeSettings>()

                ;
        }

        protected override void RegisterDependencies()
        {
            RegisterDependencies(Container);
        }
    }
}