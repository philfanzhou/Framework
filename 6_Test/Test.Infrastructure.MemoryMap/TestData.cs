using Framework.Infrastructure.MemoryMap;
using System;

namespace Test.Infrastructure.MemoryMap
{
    public struct FileHeader : IMemoryMappedFileHeader
    {
        public int DataCount { get; set; }

        public int MaxDataCount { get; set; }

        private String64 stockCode;
        public string StockCode
        {
            get { return stockCode.Value; }
            set { stockCode.Value = value; }
        }

        private String128 stockName;
        public string StockName
        {
            get { return stockName.Value; }
            set { stockName.Value = value; }
        }

        public String256 StockComment;

        public String256 Reserved1;

        public String256 Reserved2;

        public String256 Reserved3;

        public String256 Reserved4;

        public String256 Reserved5;

        public String256 Reserved6;
    }

    public struct DataItem
    {
        public String64 StockCode;

        public String128 StockName;

        public String256 StockComment;

        public int IntData { get; set; }

        public float FloatData { get; set; }

        public double DoubleData { get; set; }

        public decimal DecimalData { get; set; }

        public long LongData { get; set; }

        public DataItem2 OtherStruct { get; set; }

        public double Amount { get; set; }

        public DateTime Time { get; set; }

        private int enumField;
        public TestEnum Enum
        {
            get
            {
                if (this.enumField == 1)
                {
                    return TestEnum.U2;
                }
                else
                {
                    return TestEnum.U1;
                }
            }
            set
            {
                if (value == TestEnum.U2)
                {
                    this.enumField = 1;
                }
                else
                {
                    this.enumField = 0;
                }
            }
        }

        public override string ToString()
        {
            return this.DoubleData.ToString() + " " + Time.ToString("yyyy-MM-dd hh:mm:ss");
        }
    }

    public enum TestEnum
    {
        U1 = 0,
        U2 = 1,
        U3 = 3,
        U4 = 4,
        U5 = 5,

    }

    public struct DataItem2
    {
        public int IntData { get; set; }

        public double DoubleData { get; set; }
    }

    public class DataFile
        : NonConcurrentMemoryMappedFile<FileHeader, DataItem>
    {
        public DataFile(string path) : base(path) { }

        public DataFile(string path, FileHeader header) : base(path, header) { }

        public static DataFile Open(string path)
        {
            return new DataFile(path);
        }

        public static DataFile Create(string path, FileHeader fileHeader)
        {
            return new DataFile(path, fileHeader);
        }
    }


}
