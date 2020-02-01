using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

using Unity;

namespace TFrameWork.CallCenter.WCF.Service
{
    public class DependencyInjectionInstanceProvider : IInstanceProvider
    {
        readonly IUnityContainer _container;
        readonly Type _serviceType;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyInjectionInstanceProvider"/> class.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        public DependencyInjectionInstanceProvider(IUnityContainer container, Type serviceType)
        {
            if (container == null)
                throw new ArgumentNullException();

            if (serviceType == null)
                throw new ArgumentNullException();

            _container = container;
            _serviceType = serviceType;
        }

        /// <summary>
        /// Returns a service object given the specified <see cref="T:System.ServiceModel.InstanceContext"/> object.
        /// </summary>
        /// <param name="instanceContext">The current <see cref="T:System.ServiceModel.InstanceContext"/> object.</param>
        /// <returns>A user-defined service object.</returns>
        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        /// <summary>
        /// Returns a service object given the specified <see cref="T:System.ServiceModel.InstanceContext"/> object.
        /// </summary>
        /// <param name="instanceContext">The current <see cref="T:System.ServiceModel.InstanceContext"/> object.</param>
        /// <param name="message">The message that triggered the creation of a service object.</param>
        /// <returns>The service object.</returns>
        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return _container.Resolve(_serviceType);
        }

        /// <summary>
        /// Called when an <see cref="T:System.ServiceModel.InstanceContext"/> object recycles a service object.
        /// </summary>
        /// <param name="instanceContext">The service's instance context.</param>
        /// <param name="instance">The service object to be recycled.</param>
        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
        }
    }

    public class DependencyInjectionServiceBehavior : IServiceBehavior
    {
        readonly IUnityContainer _container;

        public DependencyInjectionServiceBehavior(IUnityContainer container)
        {
            if (container == null)
                throw new ArgumentNullException();

            _container = container;
        }

        #region IServiceBehavior Members

        /// <summary>
        /// Provides the ability to pass custom data to binding elements to support the contract implementation.
        /// </summary>
        /// <param name="serviceDescription">The service description of the service.</param>
        /// <param name="serviceHostBase">The host of the service.</param>
        /// <param name="endpoints">The service endpoints.</param>
        /// <param name="bindingParameters">Custom objects to which binding elements have access.</param>
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        { }

        /// <summary>
        /// Provides the ability to change run-time property values or insert custom extension objects such as error handlers, message or parameter interceptors, security extensions, and other custom extension objects.
        /// </summary>
        /// <param name="serviceDescription">The service description.</param>
        /// <param name="serviceHostBase">The host that is currently being built.</param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            //foreach (var cdb in serviceHostBase.ChannelDispatchers)
            //{
            //    var cd = cdb as ChannelDispatcher;

            //    if (cd != null)
            //    {
            //        foreach (var ed in cd.Endpoints)
            //        {
            //            ed.DispatchRuntime.InstanceProvider = new DependencyInjectionInstanceProvider(serviceDescription.ServiceType);
            //        }
            //    }
            //}

            foreach (ChannelDispatcher channelDispatcher in serviceHostBase.ChannelDispatchers)
            {
                foreach (EndpointDispatcher endpointDispatcher in channelDispatcher.Endpoints)
                {
                    if (endpointDispatcher.ContractName != "IMetadataExchange")
                    {
                        string contractName = endpointDispatcher.ContractName;

                        ServiceEndpoint serviceEndpoint = serviceDescription.Endpoints.FirstOrDefault(e => e.Contract.Name == contractName);

                        if (serviceEndpoint != null)
                        {
                            endpointDispatcher.DispatchRuntime.InstanceProvider = new DependencyInjectionInstanceProvider(_container, serviceEndpoint.Contract.ContractType);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Provides the ability to inspect the service host and the service description to confirm that the service can run successfully.
        /// </summary>
        /// <param name="serviceDescription">The service description.</param>
        /// <param name="serviceHostBase">The service host that is currently being constructed.</param>
        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        { }

        #endregion
    }

    public class DependencyInjectionServiceHost : ServiceHost
    {
        readonly IUnityContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyInjectionServiceHost"/> class.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="baseAddresses">The base addresses.</param>
        public DependencyInjectionServiceHost(IUnityContainer container, Type serviceType, Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            if (container == null)
                throw new ArgumentNullException();

            _container = container;
        }

        /// <summary>
        /// Opens the channel dispatchers.
        /// </summary>
        /// <param name="timeout">The <see cref="T:System.Timespan"/> that specifies how long the on-open operation has to complete before timing out.</param>
        protected override void OnOpen(TimeSpan timeout)
        {
            Description.Behaviors.Add(new DependencyInjectionServiceBehavior(_container));
            base.OnOpen(timeout);
        }
    }

    public abstract class DependencyInjectionServiceHostFactory : ServiceHostFactory
    {
        protected IUnityContainer Container { get; } = new UnityContainer();

        /// <summary>
        /// Creates a <see cref="DependencyInjectionServiceHost"/> for a specified type of service with a specific base address. 
        /// </summary>
        /// <returns>
        /// A <see cref="DependencyInjectionServiceHost"/> for the type of service specified with a specific base address.
        /// </returns>
        /// <param name="serviceType">
        /// Specifies the type of service to host. 
        /// </param>
        /// <param name="baseAddresses">
        /// The <see cref="T:System.Array"/> of type <see cref="T:System.Uri"/> that contains the base addresses for the service hosted.
        /// </param>
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            //Register the service as a type so it can be found from the instance provider
            Container.RegisterType(serviceType);

            RegisterDependencies();
            return new DependencyInjectionServiceHost(Container, serviceType, baseAddresses);
        }

        /// <summary>
        /// Initialization logic that any derived type would use to set up the ServiceLocator provider.  Look to UnityServiceHostFactory to see how this is done, if implementing for 
        /// another IoC engine.
        /// </summary>
        protected abstract void RegisterDependencies();
    }
}
