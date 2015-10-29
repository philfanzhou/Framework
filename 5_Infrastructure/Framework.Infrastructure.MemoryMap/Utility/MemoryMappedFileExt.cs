using System.IO;
using System.IO.MemoryMappedFiles;

namespace Framework.Infrastructure.MemoryMap
{
    internal class MemoryMappedFileExt
    {
        internal static MemoryMappedFile CreateMemoryMappedFile(string fullPath, long capacity)
        {
            MemoryMappedFile result;
            bool createNewFile = capacity > 0;
            string mapName = fullPath.GetHashCode().ToString();
            if (createNewFile)
            {
                string directory = Path.GetDirectoryName(fullPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // FileMode一定要使用CreateNew，否则可能出现覆盖文件的情况
                result = MemoryMappedFile.CreateFromFile(fullPath, FileMode.CreateNew, mapName, capacity);
            }
            else
            {
                if (!TryOpenExisting(mapName, out result))
                {
                    result = MemoryMappedFile.CreateFromFile(fullPath, FileMode.Open, mapName, capacity);
                }
            }

            return result;
        }

        private static bool TryOpenExisting(string mapName, out MemoryMappedFile memoryMappedFile)
        {
            bool result = false;
            MemoryMappedFile mmf = null;
            try
            {
                mmf = MemoryMappedFile.OpenExisting(mapName);
            }
            catch
            {
                mmf = null;
            }
            finally
            {
                result = mmf != null;
                memoryMappedFile = mmf;
            }

            return result;
        }
    }
}
