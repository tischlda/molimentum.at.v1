using System;

namespace Molimentum.Providers.Google
{
    static class Convert
    {
        public static readonly DateTime c_baseDateTime = new DateTime(1970, 1, 1);

        public static DateTime? FromTimestamp(string s)
        {
            Int64 result;

            if (Int64.TryParse(s, out result)) return c_baseDateTime.AddMilliseconds(result);

            return null;
        }

        
        public static UInt32? ToNullableUInt32(string s)
        {
            UInt32 result;

            if (UInt32.TryParse(s, out result)) return result;

            return null;
        }

        public static string ToTimestamp(DateTime value)
        {
            return (value.Millisecond - c_baseDateTime.Millisecond).ToString();
        }
    }
}
