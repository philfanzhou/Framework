using System.IO;

namespace Framework.Infrastructure.MemoryMappedFile
{
    internal class RightDataMover : DataMover
    {
        public RightDataMover(long position, long destination, long length)
            : base(position, destination, length)
        { }

        public override void Move(System.IO.MemoryMappedFiles.MemoryMappedFile mmf)
        {
            using (var stream = mmf.CreateViewStream())
            {
                byte[] buffer = new byte[BufferSize];
                while (Length > 0)
                {
                    // 如果待移动数据长度小于buffer，将数据长度作为buffer长度
                    if (Length < BufferSize)
                    {
                        buffer = new byte[Length];
                    }

                    // 右移数据，, 是从数据的最右端开始移动
                    // 需要重新计算当前移动数据的实际位置
                    // 实际目的地址 = 原目的地址 - buffer长度
                    Destination -= buffer.Length;
                    // 待移动数据实际位置 = 原数据位置 - buffer长度
                    Position -= buffer.Length;


                    // 游标回到数据所在位置并读取数据进入buffer
                    stream.Seek(Position, SeekOrigin.Begin);
                    stream.Read(buffer, 0, buffer.Length);

                    // 游标移动到目的地址，并写入buffer数据
                    stream.Seek(Destination, SeekOrigin.Begin);
                    stream.Write(buffer, 0, buffer.Length);

                    // 移动数据之后，待移动数据的长度要随之减少
                    Length -= buffer.Length;
                }
            }
        }
    }
}
