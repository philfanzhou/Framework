using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework.Infrastructure.MemoryMap;

namespace Test.Infrastructure.MemoryMap
{
    [TestClass]
    public class StringStructTest
    {
        private static void AssertValue<T>(T t, string value)
            where T : HasStringValue
        {
            t.Value = value;
            Assert.AreEqual(value, t.Value);
            Assert.AreEqual(value, t.ToString());
        }

        private static void TestNullandEmpty<T>(T t)
            where T : HasStringValue
        {
            AssertValue(t, null);
            AssertValue(t, string.Empty);
        }

        private static void Test64<T>(T t)
            where T : HasStringValue
        {
            AssertValue(t, "      ");
            AssertValue(t, "600036");
            AssertValue(t, "Aa345&");
            AssertValue(t, "测试");
            AssertValue(t, "招商银行");
            AssertValue(t, "\0\0\0\0\0\0\0\0");
            AssertValue(t, "\01\r\n2\0");
            AssertValue(t, "~!@#$%^&");
            AssertValue(t, "*()_+");
            AssertValue(t, "`123456");
            AssertValue(t, "7890-=");
            AssertValue(t, "qwertyui");
            AssertValue(t, "op[]\\");
            AssertValue(t, "{}|");
            AssertValue(t, "asdfghjk");
            AssertValue(t, "l;':\"");
            AssertValue(t, "zxcvbnm");
            AssertValue(t, ",./<>?");
            AssertValue(t, "（）【】");
        }

        private static void Test128<T>(T t)
            where T : HasStringValue
        {
            AssertValue(t, "一二三四五六七八");
        }

        private static void Test256<T>(T t)
            where T : HasStringValue
        {
            AssertValue(t, "一二三四五六七八九十一二三四五六");
        }

        [TestMethod]
        public void TestString64()
        {
            String64 string64 = new String64();

            TestNullandEmpty(string64);
            Test64(string64);
        }

        [TestMethod]
        public void TestString128()
        {
            String128 string128 = new String128();

            TestNullandEmpty(string128);
            Test64(string128);
            Test128(string128);
        }

        [TestMethod]
        public void TestString256()
        {
            String256 string256 = new String256();

            TestNullandEmpty(string256);
            Test64(string256);
            Test128(string256);
            Test256(string256);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestString64Length()
        {
            String64 string64 = new String64();
            string64.Value = "一二三四a";
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestString128Length()
        {
            String128 string128 = new String128();
            string128.Value = "一二三四五六七八a";
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestString256Length()
        {
            String256 string256 = new String256();
            string256.Value = "一二三四五六七八九十一二三四五六a";
        }
    }
}
