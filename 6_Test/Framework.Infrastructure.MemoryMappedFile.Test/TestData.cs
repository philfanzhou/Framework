using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Framework.Infrastructure.MemoryMappedFile.Test
{
    public struct FileHeader : IMemoryMappedFileHeader
    {
        public int DataCount { get; set; }

        public int MaxDataCount { get; set; }
        
        private String64 StockCode;

        public String128 StockName;

        public String256 StockComment;

        public string Code
        {
            get { return this.StockCode.Value; }
            set { this.StockCode.Value = value; }
        }
    }

    public struct DataItem
    {
        public int IntData { get; set; }

        public float FloatData { get; set; }

        public double DoubleData { get; set; }

        public decimal DecimalData { get; set; }

        public long LongData { get; set; }

        public DataItem2 OtherStruct { get; set; }

        public double Amount { get; set; }

        public DateTime Time { get; set; }

        public override string ToString()
        {
            return this.DoubleData.ToString() + " " + Time.ToString("yyyy-MM-dd hh:mm:ss");
        }
    }

    public struct DataItem2
    {
        public int IntData { get; set; }

        public double DoubleData { get; set; }
    }

    public class DataFile
        : MemoryMappedFileBase<FileHeader, DataItem>
    {
        private DataFile() { }
    }
}
