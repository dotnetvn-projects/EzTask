﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EzTask.Framework.Common
{
    public static class EnumUtilities
    {
        public static T ToEnum<T>(this Int16 value)
        {
            return (T)Enum.ToObject(typeof(T), value);
        }

        public static Int16 ToInt16<T>(this Enum value)
        {
            return Convert.ToInt16(value);
        }

        public static T ToEnum<T>(this string value)
        {
           return (T)Enum.Parse(typeof(T), value);
        }

        public static List<string> ToList<T>()
        {
            var items = Enum.GetNames(typeof(T));
            return items.ToList();
        }
    }
}