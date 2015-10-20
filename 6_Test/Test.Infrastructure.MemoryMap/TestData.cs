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

    /// <summary>
    /// 交易市场枚举
    /// </summary>
    public enum Market
    {
        Unknown = 0,

        /// <summary>
        /// 上海证券交易所
        /// </summary>
        XSHG = 1,

        /// <summary>
        /// 深圳证券交易所
        /// </summary>
        XSHE = 2,

        /// <summary>
        /// 中国金融期货交易所
        /// </summary>
        CCFX = 3,

        /// <summary>
        /// 大连商品交易所
        /// </summary>
        XDCE = 4,

        /// <summary>
        /// 上海期货交易所
        /// </summary>
        XSGE = 5,

        /// <summary>
        /// 郑州商品交易所
        /// </summary>
        XZCE = 6,

        /// <summary>
        /// 香港证券交易所
        /// </summary>
        XHKG = 7
    }

    public class SinaRealTimeData
    {
        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 交易市场
        /// </summary>
        public Market Market { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 今开
        /// </summary>
        public double TodayOpen { get; set; }

        /// <summary>
        /// 昨收
        /// </summary>
        public double YesterdayClose { get; set; }

        /// <summary>
        /// 成交价
        /// </summary>
        public double Current { get; set; }

        /// <summary>
        /// 最高
        /// </summary>
        public double High { get; set; }

        /// <summary>
        /// 最低
        /// </summary>
        public double Low { get; set; }

        /// <summary>
        /// 成交量
        /// </summary>
        public double Volume { get; set; }

        /// <summary>
        /// 成交额
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// 日期与时间
        /// </summary>
        public DateTime Time { get; set; }

        #region 卖五
        /// <summary>
        /// 卖五价
        /// </summary>
        public double Sell5Price { get; set; }

        /// <summary>
        /// 卖五量
        /// </summary>
        public double Sell5Volume { get; set; }

        /// <summary>
        /// 卖四价
        /// </summary>
        public double Sell4Price { get; set; }

        /// <summary>
        /// 卖四量
        /// </summary>
        public double Sell4Volume { get; set; }

        /// <summary>
        /// 卖三价
        /// </summary>
        public double Sell3Price { get; set; }

        /// <summary>
        /// 卖三量
        /// </summary>
        public double Sell3Volume { get; set; }

        /// <summary>
        /// 卖二价
        /// </summary>
        public double Sell2Price { get; set; }

        /// <summary>
        /// 卖二量
        /// </summary>
        public double Sell2Volume { get; set; }

        /// <summary>
        /// 卖一价
        /// </summary>
        public double Sell1Price { get; set; }

        /// <summary>
        /// 卖一量
        /// </summary>
        public double Sell1Volume { get; set; }
        #endregion

        #region 买五
        /// <summary>
        /// 买一价
        /// </summary>
        public double Buy1Price { get; set; }

        /// <summary>
        /// 买一量
        /// </summary>
        public double Buy1Volume { get; set; }

        /// <summary>
        /// 买二价
        /// </summary>
        public double Buy2Price { get; set; }

        /// <summary>
        /// 买二量
        /// </summary>
        public double Buy2Volume { get; set; }

        /// <summary>
        /// 买三价
        /// </summary>
        public double Buy3Price { get; set; }

        /// <summary>
        /// 买三量
        /// </summary>
        public double Buy3Volume { get; set; }

        /// <summary>
        /// 买四价
        /// </summary>
        public double Buy4Price { get; set; }

        /// <summary>
        /// 买四量
        /// </summary>
        public double Buy4Volume { get; set; }

        /// <summary>
        /// 买五价
        /// </summary>
        public double Buy5Price { get; set; }

        /// <summary>
        /// 买五量
        /// </summary>
        public double Buy5Volume { get; set; }
        #endregion
    }

    public static class DataConverter
    {
        public static SinaRealTimeData GetDataFromSource(string strData)
        {
            SinaRealTimeData data = new SinaRealTimeData();
            strData = strData.Remove(0, 11);

            string market = strData.Substring(0, 2);
            if (market == "sh")
            {
                data.Market = Market.XSHG;
            }
            else if (market == "sz")
            {
                data.Market = Market.XSHE;
            }
            else
            {
                data.Market = Market.Unknown;
            }

            strData = strData.Remove(0, 2);
            data.Code = strData.Substring(0, 6);

            int startIndex = strData.IndexOf("\"") + 1;
            int length = strData.LastIndexOf("\"") - startIndex;
            strData = strData.Substring(startIndex, length);

            string[] fields = strData.Split(',');

            data.ShortName = fields[0];
            data.TodayOpen = Convert.ToDouble(fields[1]);
            data.YesterdayClose = Convert.ToDouble(fields[2]);
            data.Current = Convert.ToDouble(fields[3]);
            data.High = Convert.ToDouble(fields[4]);
            data.Low = Convert.ToDouble(fields[5]);
            data.Volume = Convert.ToDouble(fields[8]);
            data.Amount = Convert.ToDouble(fields[9]);

            data.Buy1Volume = Convert.ToDouble(fields[10]);
            data.Buy1Price = Convert.ToDouble(fields[11]);

            data.Buy2Volume = Convert.ToDouble(fields[12]);
            data.Buy2Price = Convert.ToDouble(fields[13]);

            data.Buy3Volume = Convert.ToDouble(fields[14]);
            data.Buy3Price = Convert.ToDouble(fields[15]);

            data.Buy4Volume = Convert.ToDouble(fields[16]);
            data.Buy4Price = Convert.ToDouble(fields[17]);

            data.Buy5Volume = Convert.ToDouble(fields[18]);
            data.Buy5Price = Convert.ToDouble(fields[19]);

            data.Sell1Volume = Convert.ToDouble(fields[20]);
            data.Sell1Price = Convert.ToDouble(fields[21]);

            data.Sell2Volume = Convert.ToDouble(fields[22]);
            data.Sell2Price = Convert.ToDouble(fields[23]);

            data.Sell3Volume = Convert.ToDouble(fields[24]);
            data.Sell3Price = Convert.ToDouble(fields[25]);

            data.Sell4Volume = Convert.ToDouble(fields[26]);
            data.Sell4Price = Convert.ToDouble(fields[27]);

            data.Sell5Volume = Convert.ToDouble(fields[28]);
            data.Sell5Price = Convert.ToDouble(fields[29]);

            data.Time = Convert.ToDateTime(fields[30] + " " + fields[31]);

            return data;
        }

    }
}
