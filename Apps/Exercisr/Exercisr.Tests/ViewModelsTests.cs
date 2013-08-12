using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Exercisr.Contracts.ViewModels;

namespace Exercisr.Tests
{
    [TestClass]
    public class ViewModelsTests
    {
        [TestMethod]
        public void ITrackingViewModel_InitialState()
        {

            var trackingVM = DI.Container.Current.Get<ITrackingViewModel>();

           Assert.IsNotNull(trackingVM);
        }
    }
}
