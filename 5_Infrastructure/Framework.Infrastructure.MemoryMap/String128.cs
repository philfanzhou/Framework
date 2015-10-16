using System;

namespace Framework.Infrastructure.MemoryMap
{
    /// <summary>
    /// Represents a 128-bit string
    /// </summary>
    public struct String128 : HasStringValue
    {
        private long data1;
        private long data2;

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
            byte[] buffer = value.ToBytes(16, out length);
            data1 = BitConverter.ToInt64(buffer, 0);
            data2 = BitConverter.ToInt64(buffer, 8);
        }

        private string GetValue()
        {
            byte[] tmp1 = BitConverter.GetBytes(data1);
            byte[] tmp2 = BitConverter.GetBytes(data2);

            byte[] buffer = new byte[tmp1.Length + tmp2.Length];

            tmp1.CopyTo(buffer, 0);
            tmp2.CopyTo(buffer, 8);

            return buffer.ConvertToString(length);
        }
    }
}
