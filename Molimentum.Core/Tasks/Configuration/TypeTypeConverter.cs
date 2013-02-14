using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace Molimentum.Tasks.Configuration
{
    public class TypeTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            Type type = null;

            if (value != null && value is string)
            {
                var s = value.ToString();

                if (s.Length != 0)
                {
                    type = Type.GetType(s, true);
                }
                else Debug.Assert(false, "WTF????");
            }

            Debug.Assert(type != null, "WTF2????");

            return type;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (value == null || value.ToString().Length == 0)
                {
                    return "(none)";
                }

                return value.ToString();
            }

            return null;
        }
    }
}