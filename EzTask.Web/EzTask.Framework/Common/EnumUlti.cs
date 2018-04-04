using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Framework.Common
{
    public static class EnumUlt
    {
        public static T ToEnum<T>(this Int16 value)
        {
            return (T)Enum.ToObject(typeof(T), value);
        }

        public static Int16 ToInt16<T>(this Enum value)
        {
            return Int16.Parse(value.ToString());
        }
    }
}
