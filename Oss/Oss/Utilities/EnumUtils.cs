using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oss.Utilities
{
    internal static class EnumUtils
    {
        private static IDictionary<Enum, StringValueAttribute> _stringValues = new Dictionary<Enum, StringValueAttribute>();

        public static string GetStringValue(this Enum value)
        {
            string output = null;
            Type type = value.GetType();
            if (_stringValues.ContainsKey(value))
            {
                return _stringValues[value].Value;
            }
            StringValueAttribute[] attrs = type.GetField(value.ToString()).GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
            if (attrs.Length > 0)
            {
                output = attrs[0].Value;
                lock (_stringValues)
                {
                    if (!_stringValues.ContainsKey(value))
                    {
                        _stringValues.Add(value, attrs[0]);
                    }
                    return output;
                }
            }
            return value.ToString();
        }
    }
}
