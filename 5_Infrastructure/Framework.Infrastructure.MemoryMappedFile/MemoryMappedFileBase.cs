using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Framework.Infrastructure.MemoryMappedFile
{
    public class MemoryMappedFileBase<TDataHeader, TDataItem> : 
        MyMemoryMappedFile,
        IMemoryMappedFile<TDataHeader, TDataItem>
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

        private TDataHeader _header;

        #endregion

        #region Constructor
        
        /// <summary>
        /// 打开文件调用的构造函数
        /// </summary>
        /// <param name="path"></param>
        public MemoryMappedFileBase(string path) : base(path)
        {
            using (var accessor = Mmf.CreateViewAccessor(0, this._headerSize))
            {
                accessor.Read(0, out _header);
            }
        }

        /// <summary>
        /// 创建文件调用的构造函数
        /// </summary>
        /// <param name="path"></param>
        /// <param name="maxDataCount"></param>
        public MemoryMappedFileBase(string path, TDataHeader fileHeader)
            : base(path, CaculateCapacity(fileHeader))
        {
            if (fileHeader.MaxDataCount <= 0)
                throw new ArgumentOutOfRangeException("fileHeader");

            // 创建文件之后要立即更新头，避免创建之后未加数据就关闭后，下次无法打开文件
            fileHeader.DataCount = 0;
            this._header = fileHeader;

            using (var accessor = Mmf.CreateViewAccessor(0, this._headerSize))
            {
                accessor.Write(0, ref _header);
            }
        }
        #endregion

        #region Property

        public TDataHeader Header
        {
            get 
            {
                ThrowIfDisposed();
                return _header;
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
            Insert(items, _header.DataCount);
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
            Delete(0, _header.DataCount);
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
            return Read(0, _header.DataCount);
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

            if (index > _header.DataCount || index < 0)
                throw new ArgumentOutOfRangeException("index");
            if (count < 0 || index + count > this._header.DataCount)
                throw new ArgumentOutOfRangeException("count");

            if(count == 0)
            {
                return new TDataItem[0];
            }

            long offset = _headerSize + _dataItemSize * index;
            TDataItem[] result = new TDataItem[count];
            using (var accessor = Mmf.CreateViewAccessor(offset, _dataItemSize * count))
            {
                accessor.ReadArray(0, result, 0, result.Length);
            }

            return result;
        }

        protected virtual void DoDelete(int index, int count)
        {
            ThrowIfDisposed();

            if (index >= _header.MaxDataCount || index < 0)
                throw new ArgumentOutOfRangeException("index");
            if (count > _header.MaxDataCount || count < 1)
                throw new ArgumentOutOfRangeException("count");
            if (index + count > _header.MaxDataCount)
                throw new ArgumentOutOfRangeException("count");

            if (index >= _header.DataCount)
            {
                return;
            }

            if (index + count < _header.DataCount)
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
                long length = (_header.DataCount - (index + count)) * _dataItemSize;

                // 移动数据
                var mover = DataMover.Create(position, destination, length);
                mover.Move(Mmf);
            }

            // 更新文件头
            UpdateDataCount(-count);
        }

        protected virtual void DoInsert(IEnumerable<TDataItem> items, int index, bool ChangeDataCount)
        {
            ThrowIfDisposed();

            if (null == items)
                throw new ArgumentNullException("items");
            if (index > _header.MaxDataCount || index < 0)
                throw new ArgumentOutOfRangeException("index");
            var array = items.ToArray();
            if (array.Length + index > _header.MaxDataCount)
                throw new ArgumentOutOfRangeException("items");

            if (ChangeDataCount)
            {
                if (array.Length + _header.DataCount > _header.MaxDataCount)
                    throw new ArgumentOutOfRangeException("items");
            }

            if (ChangeDataCount && index < _header.DataCount)
            {
                // 待移动数据所在位置(右移：从当前有效数据的末尾开始移动)
                long position = 0;
                position += _headerSize;
                position += _dataItemSize * _header.DataCount;
                // 数据需要移动到的位置
                long destination = position;
                destination += _dataItemSize*array.Length;
                // 需要移动的byte长度
                long length = _dataItemSize * (_header.DataCount - index);

                // 移动数据
                var mover = DataMover.Create(position, destination, length);
                mover.Move(Mmf);
            }

            // 插入数据
            long offset = _headerSize + _dataItemSize * index;
            using (var accessor = Mmf.CreateViewAccessor(offset, _dataItemSize * array.Length))
            {
                accessor.WriteArray(0, array, 0, array.Length);
            }

            // 更新文件头
            if (ChangeDataCount)
            {
                if (index > _header.DataCount)
                {
                    // 如果是在当前已有数据之后的位置插入，更新已有数据数量就需要特殊处理
                    // 等于是中间加入了空白数据
                    UpdateDataCount(index - _header.DataCount + array.Length - 1);
                }
                else
                {
                    UpdateDataCount(array.Length);
                }
            }
        }

        protected virtual void UpdateDataCount(int number)
        {
            _header.DataCount += number;
            using (var accessor = Mmf.CreateViewAccessor(0, _headerSize))
            {
                accessor.Write(0, ref _header);
            }
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
