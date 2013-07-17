using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Parameters;
using WindowsPhone.Contracts.Core;

namespace WindowsPhone.DI
{
    public interface IDIContainer
    {
        T Get<T>(params IInjectionParameter[] parameters);
        T Get<T>(string name, params IInjectionParameter[] parameters);

        object UnderlyingProvider { get; }
    }
}
