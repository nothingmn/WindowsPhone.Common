using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using RunKeeper8.Contracts.ViewModels;

namespace RunKeeper8.Tests
{
    [TestClass]
    public class ViewModelsTests
    {
        [TestMethod]
        public void ITrackingViewModel_InitialState()
        {

            var trackingVM = DI.Container.Current.Get<ITrackingViewModel>();

            Assert.IsTrue(trackingVM.Calories == 0);
            Assert.IsTrue(trackingVM.Distance == 0);
            Assert.IsTrue(trackingVM.Pace == 0);
            Assert.IsTrue(trackingVM.Time == 0);
        }
    }
}
