using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using WindowsPhone.Contracts.Logging;
using WindowsPhone.Contracts.Serialization;
using WindowsPhone.Contracts.Storage;

namespace WindowsPhone.Tests
{
    [TestClass]
    public class StorageTests
    {
        private ILog _log = DI.Container.Current.Get<ILog>();

        [TestMethod]
        public void TestBasicCreateFolder()
        {
            //arrange
            var group = DI.Container.Current.Get<IGroup>();


            //act
            group.Name = "Folder";
            group.Save();

            //assert
            Assert.IsNotNull(group);

        }

        private const string SharedFileName = "Hello World";

        [TestMethod]
        public void TestBasicWriteFileNoParent()
        {
            //arrange
            var file = DI.Container.Current.Get<IItem>();


            //act
            file.Name = SharedFileName;
            file.Contents = System.Text.Encoding.UTF8.GetBytes(file.Name);
            file.Save();

            //assert
            Assert.IsNotNull(file);

        }

        [TestMethod]
        public void ReadFileNoParent()
        {
            //arrange
            TestBasicWriteFileNoParent();

            //act
            var file = DI.Container.Current.Get<IItem>();
            file.Name = SharedFileName;
            file.Load().Wait();

            string data = System.Text.Encoding.UTF8.GetString(file.Contents, 0, file.Contents.Length);
            //assert
            Assert.IsNotNull(file.Contents);
            Assert.AreEqual(file.Name, data);


        }


        [TestMethod]
        public void TestFileAt1LevelDown()
        {
            //arrange
            var group = DI.Container.Current.Get<IGroup>();
            group.Name = "Root";

            var file = DI.Container.Current.Get<IItem>();
            file.Name = "Hello World";
            file.Contents = System.Text.Encoding.UTF8.GetBytes(file.Name);

            group.Items.Add(file);

            //act
            file.Save();

            //assert
            Assert.IsNotNull(file);

        }

        private byte[] sharedContents;

        [TestMethod]
        public void TestFileAt2LevelDown()
        {
            //arrange
            var group = DI.Container.Current.Get<IGroup>();
            group.Name = "Root";

            var group2 = DI.Container.Current.Get<IGroup>();
            group2.Name = "Level 1";
            group.Groups.Add(group2);

            var file = DI.Container.Current.Get<IItem>();
            file.Name = "Hello World";
            sharedContents = System.Text.Encoding.UTF8.GetBytes(file.Name);
            file.Contents = sharedContents;

            group2.Items.Add(file);

            //act
            file.Save().Wait();

            //assert
            Assert.IsNotNull(file);

        }

        //[TestMethod]
        //public void TestLoadMultiLevelFolders()
        //{
        //    TestFileAt2LevelDown();
        //    var group = DI.Container.Current.Get<IGroup>();
        //    group.Load().Wait();

        //    Debug.WriteLine(group.Groups.Count);
        //    Debug.WriteLine(group.Groups[0].Groups.Count);
        //    Debug.WriteLine(group.Groups[0].Groups[0].Items.Count);
        //    var item = group.Groups[0].Groups[0].Items[0];
        //    item.Load().Wait();



        //    string expected = System.Text.Encoding.UTF8.GetString(sharedContents, 0, sharedContents.Length);
        //    string actual = System.Text.Encoding.UTF8.GetString(item.Contents, 0, item.Contents.Length);
        //    Assert.AreEqual(expected, actual);


        //}

    }
}
