using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using WindowsPhone.Common.Membership;
using WindowsPhone.Contracts.Logging;
using WindowsPhone.Contracts.Membership;
using WindowsPhone.Contracts.Repository;
using WindowsPhone.Contracts.Serialization;

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

        public IUser GetUser()
        {
            var user = DI.Container.Current.Get<IUser>();
            user.Email = "rob.chartier@gmail.com";
            user.FirstName = "Rob";
            user.LastName = "Chartier";

            return user;
        }
        [TestMethod]
        public void AlotOfInserts()
        {

            //arrange
            var repo = DI.Container.Current.Get<IRepository>();
            var user = GetUser();
            //act

            for (int x = 0; x < 10; x++)
            {
                user.Id = 0;
                user.DisplayName = user.DisplayName + "_" + x.ToString();
                repo.Insert<User>(user).ContinueWith(t =>
                    {
                        int count = t.Result;

                        repo.Single<User>(user.Id).ContinueWith(u =>
                            {
                                //assert
                                var userFromDB = u.Result;
                                Assert.AreEqual(user.Email, userFromDB.Email);
                                Assert.AreEqual(user.FirstName, userFromDB.FirstName);
                                Assert.AreEqual(user.LastName, userFromDB.LastName);
                                Assert.AreEqual(user.Id, userFromDB.Id);


                            }).Wait();

                    }).Wait();

            }
        }

        [TestMethod]
        public void Insert_New_User()
        {
            //arrange
            var repo = DI.Container.Current.Get<IRepository>();
            var user = GetUser();
            //act
          repo.Insert<User>(user).ContinueWith(t =>
                {
                   int count = t.Result;

                   repo.Single<User>(user.Id).ContinueWith(u =>
                   {
                       //assert
                       var userFromDB = u.Result;
                       Assert.AreEqual(user.Email, userFromDB.Email);
                       Assert.AreEqual(user.FirstName, userFromDB.FirstName);
                       Assert.AreEqual(user.LastName, userFromDB.LastName);
                       Assert.AreEqual(user.Id, userFromDB.Id);



                       //clean up
                       repo.Delete<User>(userFromDB).Wait();

                   }).Wait();

                }).Wait();


        }

        [TestMethod]
        public void Delete_New_User()
        {
            //arrange
            var repo = DI.Container.Current.Get<IRepository>();

            //act
            var user = GetUser();
            repo.Insert<User>(user).ContinueWith(t =>
            {
                int count = t.Result;
                _log.InfoFormat("Delete, during insert:user.id=", user.Id);
            }).Wait();

            //clean up
            repo.Delete<User>(user).Wait();

            repo.Single<User>(user.Id).ContinueWith(t =>
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

            var user = GetUser();
            //act
            repo.Insert<User>(user).ContinueWith(t =>
                {
                    int count = t.Result;
                }).Wait();


            repo.Query<User>("select * from user where id=?", new object[]{user.Id}).ContinueWith(t =>
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
            var user = GetUser();
            repo.Insert<User>(user).ContinueWith(t =>
                {
                    int count = t.Result;
                }).Wait();


            //act
            user.DisplayName = "John Doe";
            repo.Update<User>(user, user.Id).Wait();

            repo.Query<User>("select * from user where id=?", new object[] { user.Id }).ContinueWith(t =>
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
