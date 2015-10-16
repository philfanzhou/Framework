using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.Infrastructure.Repository.EF.Config;
using Test.Infrastructure.Repository.EF.Metadata;

namespace Test.Infrastructure.Repository.EF
{
    [TestClass]
    public class SqlServerTest
    {
        public SqlServerTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            DatabaseHelper.InitializeSqlServer(true);
        }
        //
        // Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestSqlServerReadAndWrite()
        {
            Person expected = new Person
            {
                Id = 1,
                FirstName = "Hi",
                LastName = "Hello"
            };

            PersonDataService.Add(expected);
            var actual = PersonDataService.GetPerson(expected.Id);

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.FirstName, actual.FirstName);
            Assert.AreEqual(expected.LastName, actual.LastName);
        }
    }
}
