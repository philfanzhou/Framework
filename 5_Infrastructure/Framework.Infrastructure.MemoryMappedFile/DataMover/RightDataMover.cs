namespace Framework.Infrastructure.MemoryMappedFile
{
    internal class RightDataMover : DataMover
    {
        public RightDataMover(long position, long destination, long length)
            : base(position, destination, length)
        { }

        public override void Move(System.IO.MemoryMappedFiles.MemoryMappedFile mmf)
        {
            long offset = base.AbsolutePosition - base.DataLength;
            long viewLength = base.AbsoluteDestination - base.AbsolutePosition + base.DataLength;
            using (var stream = mmf.CreateViewStream(offset, viewLength))
            {
                long relativePosition = base.DataLength;
                long relativeDestination = viewLength;
                long dataLength = base.DataLength;

                while (dataLength > 0)
                {
                    // 如果待移动数据长度小于buffer，将数据长度作为buffer长度
                    if (dataLength < Buffer.Length)
                    {
                        Buffer = new byte[dataLength];
                    }

                    // 右移数据，是从数据的最右端开始移动
                    // 需要重新计算当前移动数据的实际位置
                    // 实际目的地址 = 原目的地址 - buffer长度
                    relativeDestination -= Buffer.Length;
                    // 待移动数据实际位置 = 原数据位置 - buffer长度
                    relativePosition -= Buffer.Length;

                    DoMove(relativePosition, relativeDestination, stream);

                    // 移动数据之后，待移动数据的长度要随之减少
                    dataLength -= Buffer.Length;
                }
            }
        }
    }
}
