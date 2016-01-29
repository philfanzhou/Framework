using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.Infrastructure.Repository.EF.Config;
using Test.Infrastructure.Repository.EF.Metadata;
using System.Linq;

namespace Test.Infrastructure.Repository.EF
{
    [TestClass]
    public class SqlServerTest
    {
        [TestInitialize()]
        public void MyTestInitialize()
        {
            DatabaseHelper.InitializeSqlServer(true);
        }

        [TestMethod]
        public void TestSqlServerReadAndWrite()
        {
            TestMethod.TestReadAndWrite();
        }

        [TestMethod]
        public void TestSqlServerAddRange()
        {
            TestMethod.TestAddRange();
        }

        [TestMethod]
        public void TestSqlServerPerformance()
        {
            TestMethod.PerformanceTest();
        }
    }
}
