﻿using System;

namespace Framework.Infrastructure.MemoryMappedFile.Test
{
    public struct FileHeader : IMemoryMappedFileHeader
    {
        public int DataCount { get; set; }

        public int MaxDataCount { get; set; }

        public String64 StockCode;

        public String128 StockName;

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
