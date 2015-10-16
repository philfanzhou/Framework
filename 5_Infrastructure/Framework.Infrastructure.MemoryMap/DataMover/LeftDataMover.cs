namespace Framework.Infrastructure.MemoryMap
{
    internal class LeftDataMover : DataMover
    {
        public LeftDataMover(long position, long destination, long length)
            : base(position, destination, length)
        { }

        public override void Move(System.IO.MemoryMappedFiles.MemoryMappedFile mmf)
        {
            long offset = base.AbsoluteDestination;
            long viewLength = base.AbsolutePosition - base.AbsoluteDestination + base.DataLength;

            using (var stream = mmf.CreateViewStream(offset, viewLength))
            {
                long relativePosition = base.AbsolutePosition - base.AbsoluteDestination;
                long relativeDestination = 0;
                long dataLength = base.DataLength;

                while (dataLength > 0)
                {
                    // 如果待移动数据长度小于buffer，将数据长度作为buffer长度
                    if (dataLength < Buffer.Length)
                    {
                        Buffer = new byte[dataLength];
                    }

                    DoMove(relativePosition, relativeDestination, stream);

                    // 左移数据，先进行数据移动了之后，再进行地址位置更新，为下一次移动做准备
                    relativeDestination += Buffer.Length;
                    relativePosition += Buffer.Length;

                    // 移动数据之后，待移动数据的长度要随之减少
                    dataLength -= Buffer.Length;
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
