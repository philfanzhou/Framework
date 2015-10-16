namespace Framework.Infrastructure.MemoryMappedFile
{
    internal abstract class DataMover
    {
        protected long Position { get; set; }
        protected long Destination { get; set; }
        protected long Length { get; set; }
        protected int BufferSize { get; private set; }

        protected DataMover(long position, long destination, long length)
        {
            Position = position;
            Destination = destination;
            Length = length;

            // 计算bufferSize
            BufferSize = 1024 * 512; // 512K
        }

        public abstract void Move(System.IO.MemoryMappedFiles.MemoryMappedFile mmf);

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
