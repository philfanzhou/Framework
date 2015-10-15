using System;

namespace Framework.Infrastructure.MemoryMappedFile
{
    /// <summary>
    /// Represents a 256-bit string
    /// </summary>
    public struct String256 : HasStringValue
    {
        private long data1;
        private long data2;
        private long data3;
        private long data4;

        private short length;

        public string Value
        {
            get
            {
                if (length == -1)
                {
                    return null;
                }
                else if (length == 0)
                {
                    return string.Empty;
                }
                else
                {
                    return GetValue();
                }
            }
            set
            {
                if (null == value)
                {
                    length = -1;
                }
                else if (value == string.Empty)
                {
                    length = 0;
                }
                else
                {
                    SetValue(value);
                }
            }
        }

        public override string ToString()
        {
            return Value;
        }

        private void SetValue(string value)
        {
            byte[] buffer = value.ToBytes(32, out length);
            data1 = BitConverter.ToInt64(buffer, 0);
            data2 = BitConverter.ToInt64(buffer, 8);
            data3 = BitConverter.ToInt64(buffer, 16);
            data4 = BitConverter.ToInt64(buffer, 24);
        }

        private string GetValue()
        {
            byte[] tmp1 = BitConverter.GetBytes(data1);
            byte[] tmp2 = BitConverter.GetBytes(data2);
            byte[] tmp3 = BitConverter.GetBytes(data3);
            byte[] tmp4 = BitConverter.GetBytes(data4);

            byte[] buffer = new byte[tmp1.Length + tmp2.Length + tmp3.Length + tmp4.Length];

            tmp1.CopyTo(buffer, 0);
            tmp2.CopyTo(buffer, 8);
            tmp3.CopyTo(buffer, 16);
            tmp4.CopyTo(buffer, 24);

            return buffer.ConvertToString(length);
        }
    }
}
