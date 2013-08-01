using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using WindowsPhone.Contracts.Logging;
using WindowsPhone.Contracts.Repository;
using WindowsPhone.Contracts.Serialization;
using WindowsPhone.Data.DTO;

namespace WindowsPhone.Tests
{
    [TestClass]
    public class DataTests
    {
        private ILog _log = DI.Container.Current.Get<ILog>();

        [TestMethod]
        public void query_for_a_userid_that_doesnt_exist()
        {
            //arrange
            var repo = DI.Container.Current.Get<IRepository>();

            //act
            repo.Single<User>(1).ContinueWith(t =>
            {
                //assert
                Assert.IsNull(t.Result);
            }).Wait();
        }

        [TestMethod]
        public void Insert_New_User()
        {
            //arrange
            var repo = DI.Container.Current.Get<IRepository>();

            //act
            int id = -1;
            var user = new User() { Email="rob@home.com", FirstName="Rob", LastName="C"};
            repo.Insert<User>(user).ContinueWith(t =>
                {
                   id = t.Result;
                    user.Id = id;
                }).Wait();

            repo.Single<User>(id).ContinueWith(t =>
                {
                    //assert
                    var userFromDB = t.Result;
                    Assert.AreEqual(user.Email, userFromDB.Email);
                    Assert.AreEqual(user.FirstName, userFromDB.FirstName);
                    Assert.AreEqual(user.LastName, userFromDB.LastName);
                    Assert.AreEqual(user.Id, userFromDB.Id);

                }).Wait();

            //clean up
            repo.Delete<User>(user).Wait();

        }

        [TestMethod]
        public void Delete_New_User()
        {
            //arrange
            var repo = DI.Container.Current.Get<IRepository>();

            //act
            int id = -1;
            var user = new User() { Email = "rob@home.com", FirstName = "Rob", LastName = "C" };
            repo.Insert<User>(user).ContinueWith(t =>
            {
                id = t.Result;
                user.Id = id;
            }).Wait();

            //clean up
            repo.Delete<User>(user).Wait();

            repo.Single<User>(id).ContinueWith(t =>
            {
                //assert
                Assert.IsNull(t.Result);

            }).Wait();

        }
        [TestMethod]
        public void Query_New_User()
        {
            //arrange
            var repo = DI.Container.Current.Get<IRepository>();

            //act
            int id = -1;
            var user = new User() { Email = "rob@home.com", FirstName = "Rob", LastName = "C" };
            repo.Insert<User>(user).ContinueWith(t =>
            {
                id = t.Result;
                user.Id = id;
            }).Wait();


            repo.Query<User>("select * from user where id=?", new object[]{id}).ContinueWith(t =>
            {
                //assert
                var userFromDB = t.Result[0];

                Assert.AreEqual(user.Email, userFromDB.Email);
                Assert.AreEqual(user.FirstName, userFromDB.FirstName);
                Assert.AreEqual(user.LastName, userFromDB.LastName);
                Assert.AreEqual(user.Id, userFromDB.Id);


            }).Wait();



            //clean up
            repo.Delete<User>(user).Wait();

        }

        [TestMethod]
        public void Update_New_User()
        {
            //arrange
            var repo = DI.Container.Current.Get<IRepository>();

            //act
            int id = -1;
            var user = new User() { Email = "rob@home.com", FirstName = "Rob", LastName = "C" };
            repo.Insert<User>(user).ContinueWith(t =>
            {
                id = t.Result;
                user.Id = id;
            }).Wait();


            //act
            user.DisplayName = "John Doe";
            repo.Update<User>(user, user.Id).Wait();

            repo.Query<User>("select * from user where id=?", new object[] { id }).ContinueWith(t =>
            {
                //assert
                var userFromDB = t.Result[0];

                Assert.AreEqual(user.Email, userFromDB.Email);
                Assert.AreEqual(user.FirstName, userFromDB.FirstName);
                Assert.AreEqual(user.LastName, userFromDB.LastName);
                Assert.AreEqual(user.Id, userFromDB.Id);


            }).Wait();



            //clean up
            repo.Delete<User>(user).Wait();

        }



    }
}
