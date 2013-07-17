using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Ninject.Parameters;
using WindowsPhone.Contracts.Core;

namespace WindowsPhone.DI
{
    public class MockContainer : IDIContainer
    {
        public object UnderlyingProvider { get { return _kernel; } }
        IKernel _kernel;
        public MockContainer()
        {
            _kernel = new StandardKernel(

                );

        }

        public T Get<T>(params IInjectionParameter[] parameters)
        {
            var parameterList = Container.DomainToNinjectParameters(parameters);
            if (parameterList != null) return _kernel.Get<T>(parameterList.ToArray());
            return _kernel.Get<T>();
        }
        public T Get<T>(string name, params IInjectionParameter[] parameters)
        {
            var parameterList = Container.DomainToNinjectParameters(parameters);
            if (parameterList != null) return _kernel.Get<T>(name, parameterList.ToArray());
            return _kernel.Get<T>(name);
        }
    }
}
