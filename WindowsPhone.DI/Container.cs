using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Ninject.Parameters;
using WindowsPhone.Contracts.Core;

namespace WindowsPhone.DI
{
    public class Container : IDIContainer
    {
        public object UnderlyingProvider { get { return Current.UnderlyingProvider; } }
        public static string DIContainer { get; set; }
        private static IDIContainer container;

        private static IDIContainer LoadFromConfig()
        {

            IDIContainer c;
            if (!string.IsNullOrEmpty(DIContainer))
            {
                var type = System.Type.GetType(DIContainer);
                if (type != null)
                {
                    c = (type.Assembly.CreateInstance(type.FullName) as IDIContainer);
                }
                else
                {
                    c = new StandardContainer();
                }
            }
            else
            {
                c = new StandardContainer();
            }
            return c;
        }

        public static object _lock = new object();

        public static IDIContainer Current
        {
            get
            {
                lock (_lock)
                {
                    if (container == null)
                    {
                        container = LoadFromConfig();
                    }
                    return container;
                }
            }
        }

        public T Get<T>(params IInjectionParameter[] parameters)
        {
            return container.Get<T>(parameters);
        }

        public T Get<T>(string name, params IInjectionParameter[] parameters)
        {
            return container.Get<T>(name, parameters);
        }

        private static object _parameterLock = new object();

        public static List<Ninject.Parameters.IParameter> DomainToNinjectParameters(params IInjectionParameter[] parameters)
        {
            if (parameters == null || parameters.Length == 0) return null;
            lock (_parameterLock)
            {
                List<IParameter> paramList = new List<IParameter>();
                foreach (var p in parameters)
                {
                    paramList.Add(new Parameter(p.Name, p.Value, p.ShouldInherit));
                }
                return paramList;
            }
        }
    }
}