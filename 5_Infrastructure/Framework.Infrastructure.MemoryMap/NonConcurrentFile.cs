using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;

namespace Framework.Infrastructure.MemoryMap
{
    public class NonConcurrentFile<TDataHeader, TDataItem> : 
        MemoryMappedFileBase,
        IMemoryMappedFile<TDataHeader>,
        IMemoryMappedFileModifier<TDataHeader, TDataItem>,
        IMemoryMappedFileReader<TDataHeader, TDataItem>
        where TDataHeader : struct, IMemoryMappedFileHeader
        where TDataItem : struct
    {
        #region Field
        /// <summary>
        /// 头文件长度
        /// </summary>
        private readonly int _headerSize = Marshal.SizeOf(typeof(TDataHeader));
        /// <summary>
        /// 单个数据长度
        /// </summary>
        private readonly int _dataItemSize = Marshal.SizeOf(typeof(TDataItem));
        #endregion

        #region Constructor

        /// <summary>
        /// 打开文件调用的构造函数
        /// </summary>
        /// <param name="path"></param>
        public NonConcurrentFile(string path) : base(path) { }

        /// <summary>
        /// 创建文件调用的构造函数
        /// </summary>
        /// <param name="path"></param>
        /// <param name="maxDataCount"></param>
        public NonConcurrentFile(string path, TDataHeader fileHeader)
            : base(path, CaculateCapacity(fileHeader))
        {
            if (fileHeader.MaxDataCount <= 0)
                throw new ArgumentOutOfRangeException("fileHeader");

            // 创建文件之后要立即更新头，避免创建之后未加数据就关闭后，下次无法打开文件
            fileHeader.DataCount = 0;
            WriteData(0, new TDataHeader[] { fileHeader });
        }
        #endregion

        #region Property

        public TDataHeader Header
        {
            get
            {
                ThrowIfDisposed();
                return ReadData<TDataHeader>(0, 1).FirstOrDefault();
            }
        }

        #endregion

        #region IMemoryMappedFile Members

        public virtual void Add(TDataItem item)
        {
            Add(new[] { item });
        }

        public virtual void Add(IEnumerable<TDataItem> items)
        {
            Insert(items, Header.DataCount);
        }

        public virtual void Delete(int index)
        {
            Delete(index, 1);
        }

        public virtual void Delete(int index, int count)
        {
            DoDelete(index, count);
        }

        public virtual void DeleteAll()
        {
            Delete(0, Header.DataCount);
        }

        public virtual void Update(TDataItem item, int index)
        {
            Update(new[] { item }, index);
        }

        public virtual void Update(IEnumerable<TDataItem> items, int index)
        {
            DoInsert(items, index, false);
        }

        public virtual TDataItem Read(int index)
        {
            return Read(index, 1).FirstOrDefault();
        }

        public virtual IEnumerable<TDataItem> Read(int index, int count)
        {
            return DoRead(index, count);
        }

        public virtual IEnumerable<TDataItem> ReadAll()
        {
            return Read(0, Header.DataCount);
        }

        public virtual void Insert(TDataItem item, int index)
        {
            Insert(new[] { item }, index);
        }

        public virtual void Insert(IEnumerable<TDataItem> items, int index)
        {
            DoInsert(items, index, true);
        }

        #endregion

        #region Protected Method
        protected virtual IEnumerable<TDataItem> DoRead(int index, int count)
        {
            ThrowIfDisposed();
            TDataHeader header = this.Header;
            if (index > header.DataCount || index < 0)
                throw new ArgumentOutOfRangeException("index");

            if (count < 0 || index + count > header.DataCount)
                throw new ArgumentOutOfRangeException("count");

            if(count == 0)
            {
                return new TDataItem[0];
            }

            long offset = _headerSize + _dataItemSize * index;
            return ReadData<TDataItem>(offset, count);
        }

        protected virtual void DoInsert(IEnumerable<TDataItem> items, int index, bool ChangeDataCount)
        {
            ThrowIfDisposed();

            if (null == items)
                throw new ArgumentNullException("items");

            TDataHeader header = this.Header;
            if (index > header.MaxDataCount || index < 0)
                throw new ArgumentOutOfRangeException("index");

            var array = items.ToArray();
            if (array.Length + index > header.MaxDataCount)
                throw new ArgumentOutOfRangeException("items");

            if (ChangeDataCount)
            {
                if (array.Length + header.DataCount > header.MaxDataCount)
                    throw new ArgumentOutOfRangeException("items");
            }

            if (ChangeDataCount && index < header.DataCount)
            {
                // 待移动数据所在位置(右移：从当前有效数据的末尾开始移动)
                long position = 0;
                position += _headerSize;
                position += _dataItemSize * header.DataCount;
                // 数据需要移动到的位置
                long destination = position;
                destination += _dataItemSize*array.Length;
                // 需要移动的byte长度
                long length = _dataItemSize * (header.DataCount - index);

                // 移动数据
                MoveData(position, destination, length);
            }

            // 插入数据
            long offset = _headerSize + _dataItemSize * index;
            WriteData(offset, array);

            // 更新文件头
            if (ChangeDataCount)
            {
                if (index > header.DataCount)
                {
                    // 如果是在当前已有数据之后的位置插入，更新已有数据数量就需要特殊处理
                    // 等于是中间加入了空白数据
                    int dataCount = index - header.DataCount + array.Length - 1;
                    header.DataCount += dataCount;
                    WriteData(0, new TDataHeader[] { header });
                }
                else
                {
                    header.DataCount += array.Length;
                    WriteData(0, new TDataHeader[] { header });
                }
            }
        }
        
        protected virtual void DoDelete(int index, int count)
        {
            ThrowIfDisposed();

            TDataHeader header = this.Header;
            if (index >= header.MaxDataCount || index < 0)
                throw new ArgumentOutOfRangeException("index");

            if (count > header.MaxDataCount || count < 1)
                throw new ArgumentOutOfRangeException("count");
            if (index + count > header.MaxDataCount)
                throw new ArgumentOutOfRangeException("count");

            if (index >= header.DataCount)
            {
                return;
            }

            if (index + count < header.DataCount)
            {
                // 待移动数据所在位置(左移)
                long position = 0;
                position += _headerSize;
                position += _dataItemSize * (index + count);
                // 数据需要移动到的位置(左移)
                long destination = 0;
                destination += _headerSize;
                destination += _dataItemSize * index;
                // 待向前移动byte长度
                long length = (header.DataCount - (index + count)) * _dataItemSize;

                // 移动数据
                MoveData(position, destination, length);
            }

            // 更新文件头
            header.DataCount += -count;
            WriteData(0, new TDataHeader[] { header });
        }
        #endregion

        #region Pirvate Method
        private static long CaculateCapacity(TDataHeader fileHeader)
        {
            return fileHeader.MaxDataCount * Marshal.SizeOf(typeof(TDataItem)) + Marshal.SizeOf(typeof(TDataHeader));
        }
        #endregion
    }
}
