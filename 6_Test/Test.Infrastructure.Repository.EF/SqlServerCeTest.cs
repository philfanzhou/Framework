using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.Infrastructure.Repository.EF.Config;
using Test.Infrastructure.Repository.EF.Metadata;

namespace Test.Infrastructure.Repository.EF
{
    [TestClass]
    public class SqlServerCeTest
    {

        [TestInitialize()]
        public void MyTestInitialize()
        {
            DatabaseHelper.InitializeSqlCe(true);
        }

        [TestMethod]
        public void TestSqlCeReadAndWrite()
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
