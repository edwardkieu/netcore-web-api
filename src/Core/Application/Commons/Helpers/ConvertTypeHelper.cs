using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Application.Commons.Helpers
{
    public static class ConvertTypeHelper
    {
        public static int ToInt(object obj, int defaultValue)
        {
            try { return int.Parse(obj.ToString()); }
            catch { return defaultValue; }
        }

        public static int ToInt(this object obj) => ToInt(obj, -1);

        public static double ToDouble(object obj, double defaultValue)
        {
            try { return double.Parse(obj.ToString()); }
            catch { return defaultValue; }
        }

        public static double ToDouble(this object obj) => ToDouble(obj, -1);

        public static float ToFloat(object obj, float defaultValue)
        {
            try { return float.Parse(obj.ToString()); }
            catch { return defaultValue; }
        }

        public static float ToFloat(this object obj) => ToFloat(obj, -1);

        public static short ToShort(object obj, short defaultValue)
        {
            try { return short.Parse(obj.ToString()); }
            catch { return defaultValue; }
        }

        public static short ToShort(this object obj) => ToShort(obj, -1);

        public static long ToLong(object obj, long defaultValue)
        {
            try { return long.Parse(obj.ToString()); }
            catch { return defaultValue; }
        }

        public static long ToLong(this object obj) => ToLong(obj, -1);

        public static bool ToBool(object obj, bool defaultValue)
        {
            try { return Convert.ToBoolean(obj.ToString()); }
            catch { return defaultValue; }
        }

        public static bool ToBool(this object obj) => ToBool(obj, false);

        public static decimal ToDecimal(object obj, decimal defaultValue)
        {
            try { return Convert.ToDecimal(obj.ToString()); }
            catch { return defaultValue; }
        }

        public static decimal ToDecimal(this object obj) => ToDecimal(obj, 0);

        public static string ToString(object obj)
        {
            return obj?.ToString() ?? string.Empty;
        }

        public static Dictionary<string, object> ToDictionary(this object obj)
        {
            return obj
                .GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(obj, null));
        }
    }
}