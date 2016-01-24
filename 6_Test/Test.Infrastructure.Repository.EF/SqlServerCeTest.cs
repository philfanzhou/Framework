using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.Infrastructure.Repository.EF.Config;
using Test.Infrastructure.Repository.EF.Metadata;
using System.Linq;

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
            TestMethod.TestReadAndWrite();
        }

        [TestMethod]
        public void TestSqlCeAddRange()
        {
            TestMethod.TestAddRange();
        }
    }
}
