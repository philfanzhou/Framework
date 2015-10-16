using System;
using System.Text;

namespace Framework.Infrastructure.MemoryMappedFile
{
    internal static class StringExt
    {
        internal static byte[] ToBytes(this string self, short bufferSize, out short dataLength)
        {
            byte[] data = Encoding.GetEncoding("GBK").GetBytes(self);
            if (data.Length > bufferSize)
            {
                throw new ArgumentOutOfRangeException();
            }

            byte[] buffer = new byte[bufferSize];
            data.CopyTo(buffer, 0);

            dataLength = Convert.ToInt16(data.Length);
            return buffer;
        }
    }
}
