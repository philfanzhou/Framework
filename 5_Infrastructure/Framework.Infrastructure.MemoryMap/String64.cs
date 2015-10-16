using System;

namespace Framework.Infrastructure.MemoryMap
{
    /// <summary>
    /// Represents a 64-bit string
    /// </summary>
    public struct String64 : HasStringValue
    {
        private long data;

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
            byte[] buffer = value.ToBytes(8, out length);
            data = BitConverter.ToInt64(buffer, 0);
        }

        private string GetValue()
        {
            return BitConverter.GetBytes(data).ConvertToString(length);
        }
    }

    public interface HasStringValue
    {
        string Value { get; set; }
    }
}
