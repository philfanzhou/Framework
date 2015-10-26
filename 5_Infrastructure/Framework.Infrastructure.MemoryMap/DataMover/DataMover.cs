using System.IO;
using System.IO.MemoryMappedFiles;

namespace Framework.Infrastructure.MemoryMap
{
    internal abstract class DataMover
    {
        #region Property
        /// <summary>
        /// 待移动数据在文件中的绝对位置
        /// </summary>
        protected long AbsolutePosition { get; private set; }
        /// <summary>
        /// 数据需要移动到的目的地绝对位置
        /// </summary>
        protected long AbsoluteDestination { get; private set; }
        /// <summary>
        /// 需要移动数据的长度，单位byte
        /// </summary>
        protected long DataLength { get; private set; }
        /// <summary>
        /// 移动数据使用的缓冲区
        /// </summary>
        protected byte[] Buffer { get; set; }
        #endregion

        protected DataMover(long position, long destination, long length)
        {
            AbsolutePosition = position;
            AbsoluteDestination = destination;
            DataLength = length;

            // 计算bufferSize
            int bufferSize = 1024 * 512; // 512K
            Buffer = new byte[bufferSize];
        }

        protected void DoMove(long position, long destination, MemoryMappedViewStream stream)
        {
            // 游标回到数据所在位置并读取数据进入buffer
            stream.Seek(position, SeekOrigin.Begin);
            stream.Read(Buffer, 0, Buffer.Length);

            // 游标移动到目的地址，并写入buffer数据
            stream.Seek(destination, SeekOrigin.Begin);
            stream.Write(Buffer, 0, Buffer.Length);
        }

        public abstract void Move(MemoryMappedFile mmf);

        public static DataMover Create(long position, long destination, long length)
        {
            // 判断移动方向
            bool isRightMove = position < destination;

            if (isRightMove)
            {
                return new RightDataMover(position, destination, length);
            }
            else
            {
                return new LeftDataMover(position, destination, length);
            }
        }
    }
}
