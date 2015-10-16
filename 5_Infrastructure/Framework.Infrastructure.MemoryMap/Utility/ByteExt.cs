using System.Text;

namespace Framework.Infrastructure.MemoryMap
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

        //protected static byte[] StructToBytes<T>(T structObj)
        //    where T : struct
        //{
        //    int size = Marshal.SizeOf(typeof(T));
        //    IntPtr buffer = Marshal.AllocHGlobal(size);
        //    try
        //    {
        //        Marshal.StructureToPtr(structObj, buffer, false);
        //        byte[] bytes = new byte[size];
        //        Marshal.Copy(buffer, bytes, 0, size);
        //        return bytes;
        //    }
        //    finally
        //    {
        //        Marshal.FreeHGlobal(buffer);
        //    }
        //}

        //protected static T BytesToStruct<T>(byte[] bytes)
        //    where T : struct
        //{
        //    int size = Marshal.SizeOf(typeof(T));
        //    IntPtr buffer = Marshal.AllocHGlobal(size);
        //    try
        //    {
        //        Marshal.Copy(bytes, 0, buffer, size);
        //        return Marshal.PtrToStructure<T>(buffer);
        //    }
        //    finally
        //    {
        //        Marshal.FreeHGlobal(buffer);
        //    }
        //}
    }
}
