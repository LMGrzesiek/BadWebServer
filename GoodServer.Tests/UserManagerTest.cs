using Microsoft.VisualStudio.TestTools.UnitTesting;
using GoodServer.Data;

namespace GoodServer.Tests
{
    [TestClass]
    public class UserManagerTest
    {
        [TestCleanup]
        public void Cleanup()
        {
            System.IO.File.Delete("users.xml");
        }

        [TestMethod]
        public void RegisterUserTest()
        {
            UserManager userManager = new UserManager();
            var actual = userManager.CreateAsync("test@test.com", "password").Result;
            Assert.AreEqual(true, actual);
        }

        [TestMethod]
        public void RegisterDuplicateUserTest()
        {
            UserManager userManager = new UserManager();
            userManager.CreateAsync("test2@test.com", "password").Wait();
            var actual = userManager.CreateAsync("test2@test.com", "password").Result;
            Assert.AreEqual(false, actual);
        }

        [TestMethod]
        public void CheckPasswordTest()
        {
            UserManager userManager = new UserManager();
            userManager.CreateAsync("test1@test.com", "password").Wait();
            var actual = userManager.CheckPasswordAsync("test1@test.com", "password").Result;
            Assert.AreEqual(true, actual);
        }

        [TestMethod]
        public void CheckSimulataneousRegistration()
        {
            UserManager userManager = new UserManager();
            bool actual1 = false, actual2 = false;
            bool t1Finished = false, t2Finished = false;
            System.Threading.Thread t1 = new System.Threading.Thread(new System.Threading.ThreadStart(() =>
            {
                userManager.CreateAsync("test3@test.com", "password").Wait();
                actual1 = userManager.CheckPasswordAsync("test3@test.com", "password").Result;
                t1Finished = true;
            }));
            System.Threading.Thread t2 = new System.Threading.Thread(new System.Threading.ThreadStart(() =>
            {
                userManager.CreateAsync("test4@test.com", "password").Wait();
                actual2 = userManager.CheckPasswordAsync("test4@test.com", "password").Result;
                t2Finished = true;

            }));

            t1.Start();
            t2.Start();

            while (!t1Finished || !t2Finished)
            {
                System.Threading.Thread.Sleep(1);
            }

            Assert.AreEqual(true, actual1);
            Assert.AreEqual(true, actual2);
        }

        //There are about 4 other possible paths in my UserManager.  I should have a unit test for each possible path.
        //Also, what happens if I try using null or empty email or password values.  What happens if I enter an invalid email?
        //What happens if I try to enter an email or password with non ASCII characters?  Greater-than/Less-than symbols?  
        //What happens if I enter the complete text of a novel as my password?
    }
}
