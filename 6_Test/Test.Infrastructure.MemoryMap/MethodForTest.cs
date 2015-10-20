using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Test.Infrastructure.MemoryMap
{
    [TestClass]
    public partial class MemoryMappedFileTest
    {
        private FileHeader CreateHeader(int maxDataCount)
        {
            FileHeader header = new FileHeader();

            header.DataCount = 0;
            header.MaxDataCount = maxDataCount;

            header.StockCode = "600036";
            header.StockName = GetRandomChinese(8);
            header.StockComment.Value = GetRandomChinese(16);
            header.Reserved1.Value = GetRandomChinese(16);
            header.Reserved2.Value = GetRandomChinese(16);

            return header;
        }

        private List<DataItem> CreateDataItem(int count)
        {
            List<DataItem> result = new List<DataItem>();
            Random random = new Random();

            for (int i = 0; i < count; i++)
            {
                var tempItem = new DataItem
                {
                    IntData = random.Next(),
                    FloatData = float.Parse(Math.Round(random.NextDouble(), 2).ToString()),
                    DoubleData = Math.Round(random.NextDouble(), 2),
                    DecimalData = Convert.ToDecimal(Math.Round(random.NextDouble(), 2)),
                    LongData = random.Next(),
                    OtherStruct = new DataItem2
                    {
                        IntData = random.Next(),
                        DoubleData = Math.Round(random.NextDouble(), 2),
                    },
                    Amount = random.Next(),
                    Time = DateTime.Now.AddSeconds(random.Next()),
                };
                tempItem.StockCode.Value = random.Next(0, 999999).ToString();
                tempItem.StockName.Value = GetRandomChinese(8);
                tempItem.StockComment.Value = GetRandomChinese(16);

                TestEnum enumValue;
                Enum.TryParse(random.Next(0, 5).ToString(), out enumValue);
                tempItem.Enum = enumValue;

                result.Add(tempItem);
            }

            return result;
        }

        private static void CompareListItem(List<DataItem> expectedList, List<DataItem> actualList)
        {
            Assert.IsNotNull(actualList);
            Assert.AreNotEqual(0, expectedList.Count);
            Assert.AreNotEqual(0, actualList.Count);
            Assert.AreEqual(expectedList.Count, actualList.Count);

            for (int i = 0; i < expectedList.Count; i++)
            {
                var expected = expectedList[i];
                var actual = actualList[i];

                //Assert.AreEqual(expected, actual);

                Assert.AreEqual(expected.StockCode.Value,
                    actual.StockCode.Value);
                Assert.AreEqual(expected.StockName.Value,
                    actual.StockName.Value);
                Assert.AreEqual(expected.StockComment.Value,
                    actual.StockComment.Value);

                Assert.IsTrue(expected.IntData - actual.IntData < 0.00000000000000001);
                Assert.IsTrue(expected.FloatData - actual.FloatData < 0.00000000000000001);
                Assert.IsTrue(expected.DoubleData - actual.DoubleData < 0.00000000000000001);
                Assert.IsTrue(expected.DecimalData - actual.DecimalData < 0.00000000000000001m);
                Assert.IsTrue(expected.LongData - actual.LongData < 0.00000000000000001);

                Assert.IsTrue(expected.OtherStruct.IntData - actual.OtherStruct.IntData < 0.00000000000000001);
                Assert.IsTrue(expected.OtherStruct.DoubleData - actual.OtherStruct.DoubleData < 0.00000000000000001);

                Assert.IsTrue(expected.Amount - actual.Amount < 0.00000000000000001);
                Assert.IsTrue(expected.Time - actual.Time < new TimeSpan(0, 0, 1));

                Assert.AreEqual(expected.Enum, actual.Enum);
            }
        }

        private string CreateFileAnyway(string fileName, int maxDataCount)
        {
            string path = Environment.CurrentDirectory + @"\" + fileName;

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (DataFile.Create(path, CreateHeader(maxDataCount)))
            { }

            return path;
        }

        private static List<DataItem> ReadAllDataFromFile(string path)
        {
            List<DataItem> actualList = new List<DataItem>();
            using (var file = DataFile.Open(path))
            {
                actualList.AddRange(file.ReadAll());
            }
            return actualList;
        }

        private List<DataItem> AddDataToFile(int maxDataCount, string path)
        {
            List<DataItem> expectedList = CreateDataItem(maxDataCount);

            // Open and add date
            using (var file = DataFile.Open(path))
            {
                file.Add(expectedList);
            }
            return expectedList;
        }

        private string GetRandomChinese(int strlength)
        {
            // 获取GB2312编码页（表） 
            Encoding gb = Encoding.GetEncoding("gb2312");

            object[] bytes = this.CreateRegionCode(strlength);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < strlength; i++)
            {
                string temp = gb.GetString((byte[])Convert.ChangeType(bytes[i], typeof(byte[])));
                sb.Append(temp);
            }

            return sb.ToString();
        }

        /** 
    此函数在汉字编码范围内随机创建含两个元素的十六进制字节数组，每个字节数组代表一个汉字，并将 
    四个字节数组存储在object数组中。 
    参数：strlength，代表需要产生的汉字个数 
    **/
        private object[] CreateRegionCode(int strlength)
        {
            //定义一个字符串数组储存汉字编码的组成元素 
            string[] rBase = new String[16] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };

            Random rnd = new Random();

            //定义一个object数组用来 
            object[] bytes = new object[strlength];

            /**
             每循环一次产生一个含两个元素的十六进制字节数组，并将其放入bytes数组中 
             每个汉字有四个区位码组成 
             区位码第1位和区位码第2位作为字节数组第一个元素 
             区位码第3位和区位码第4位作为字节数组第二个元素 
            **/
            for (int i = 0; i < strlength; i++)
            {
                //区位码第1位 
                int r1 = rnd.Next(11, 14);
                string str_r1 = rBase[r1].Trim();

                //区位码第2位 
                rnd = new Random(r1 * unchecked((int)DateTime.Now.Ticks) + i); // 更换随机数发生器的 种子避免产生重复值 
                int r2;
                if (r1 == 13)
                {
                    r2 = rnd.Next(0, 7);
                }
                else
                {
                    r2 = rnd.Next(0, 16);
                }
                string str_r2 = rBase[r2].Trim();

                //区位码第3位 
                rnd = new Random(r2 * unchecked((int)DateTime.Now.Ticks) + i);
                int r3 = rnd.Next(10, 16);
                string str_r3 = rBase[r3].Trim();

                //区位码第4位 
                rnd = new Random(r3 * unchecked((int)DateTime.Now.Ticks) + i);
                int r4;
                if (r3 == 10)
                {
                    r4 = rnd.Next(1, 16);
                }
                else if (r3 == 15)
                {
                    r4 = rnd.Next(0, 15);
                }
                else
                {
                    r4 = rnd.Next(0, 16);
                }
                string str_r4 = rBase[r4].Trim();

                // 定义两个字节变量存储产生的随机汉字区位码 
                byte byte1 = Convert.ToByte(str_r1 + str_r2, 16);
                byte byte2 = Convert.ToByte(str_r3 + str_r4, 16);
                // 将两个字节变量存储在字节数组中 
                byte[] str_r = new byte[] { byte1, byte2 };

                // 将产生的一个汉字的字节数组放入object数组中 
                bytes.SetValue(str_r, i);
            }

            return bytes;
        }
    }
}
