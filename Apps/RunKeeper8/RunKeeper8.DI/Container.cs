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
using Ninject;
using Ninject.Parameters;
using RunKeeper8.DI.Modules;
using WindowsPhone.Contracts.Core;
using WindowsPhone.DI;

namespace RunKeeper8.DI
{
    public class Container : IDIContainer
    {


        public object UnderlyingProvider
        {
            get { return CommonContainer.UnderlyingProvider; }
        }
        private static IDIContainer CommonContainer = null;
        private static object _lock = new object();
        public static IDIContainer Current 
        {
            get
            {
             lock (_lock)
             {
                 if (CommonContainer == null)
                 {
                     CommonContainer = WindowsPhone.DI.Container.Current;
                     var kernel = (CommonContainer.UnderlyingProvider as IKernel);
                     if (kernel != null)
                     {
                         kernel.Load(new ViewModelsModule());
                     }
                 }
                 return CommonContainer;
             }
            }
        }

        public T Get<T>(params WindowsPhone.Contracts.Core.IInjectionParameter[] parameters)
        {
            return CommonContainer.Get<T>(parameters);
        }

        public T Get<T>(string name, params WindowsPhone.Contracts.Core.IInjectionParameter[] parameters)
        {
            return CommonContainer.Get<T>(name, parameters);
        }

    }
}
