using System.IO;

namespace Framework.Infrastructure.MemoryMappedFile
{
    internal class LeftDataMover : DataMover
    {
        public LeftDataMover(long position, long destination, long length)
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

                    // 游标回到数据所在位置并读取数据进入buffer
                    stream.Seek(Position, SeekOrigin.Begin);
                    stream.Read(buffer, 0, buffer.Length);

                    // 游标移动到目的地址，并写入buffer数据
                    stream.Seek(Destination, SeekOrigin.Begin);
                    stream.Write(buffer, 0, buffer.Length);

                    // 左移数据，先进行数据移动了之后，再进行地址位置更新，为下一次移动做准备
                    Destination += buffer.Length;
                    Position += buffer.Length;

                    // 移动数据之后，待移动数据的长度要随之减少
                    Length -= buffer.Length;
                }

                //为了性能考虑，暂时不抹除左移后，原位置右边剩余的废弃数据
                //if (leftMove) 
                //{
                //    // 如果是左移数据，需要抹除最后面的一段位置的废弃数据
                //    buffer = new byte[position - destination];
                //    stream.Seek(destination, SeekOrigin.Begin);
                //    stream.Write(buffer, 0, buffer.Length);
                //}
            }
        }
    }
}
