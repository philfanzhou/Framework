using System.Text;

namespace Framework.Infrastructure.MemoryMappedFile
{
    internal static class ByteExt
    {
        internal static string ConvertToString(this byte[] self, short dataLength)
        {
            byte[] buffer = new byte[dataLength];
            for (short i = 0; i < dataLength; i++)
            {
                buffer[i] = self[i];
            }

            return Encoding.GetEncoding("GBK").GetString(buffer);
        }
    }
}
