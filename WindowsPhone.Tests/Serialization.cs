using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using WindowsPhone.Common;
using WindowsPhone.Contracts.Core;
using WindowsPhone.Contracts.Logging;
using WindowsPhone.Contracts.Serialization;

namespace WindowsPhone.Tests
{
    [TestClass]
    public class Serialization
    {
        private ILog _log = DI.Container.Current.Get<ILog>();

        [TestMethod]
        public void TestBasicSerialize()
        {
            //arrange
            var serializer = DI.Container.Current.Get<ISerialize>();
            var version = DI.Container.Current.Get<IVersion>();
            version.Major = 1;
            version.Minor = 2;
            version.Revision = 3;
            version.Build = 4;

            //act
            var result = serializer.Serialize(version);

            _log.Info(result);

            //assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains(":1"));
            Assert.IsTrue(result.Contains(":2"));
            Assert.IsTrue(result.Contains(":3"));
            Assert.IsTrue(result.Contains(":4"));

            //act
            var otherResult = serializer.Deserialize<AppVersion>(result);

            //assert
            //if this passes we are serializing and deserializing correctly
            Assert.AreEqual(version, otherResult);

        }
    }
}
