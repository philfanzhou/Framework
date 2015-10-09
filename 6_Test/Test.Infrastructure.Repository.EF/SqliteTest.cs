using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.Infrastructure.Repository.EF.Config;
using Test.Infrastructure.Repository.EF.Metadata;

namespace Test.Infrastructure.Repository.EF
{
    /// <summary>
    /// Summary description for SqliteTest
    /// </summary>
    [TestClass]
    public class SqliteTest
    {
        [TestInitialize()]
        public void MyTestInitialize()
        {
            DatabaseHelper.InitializeSqlite(true);
        }

        [TestMethod]
        public void TestReadAndWrite()
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
