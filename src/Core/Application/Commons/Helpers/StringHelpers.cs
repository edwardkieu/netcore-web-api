using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Application.Commons.Helpers
{
    public static class StringHelpers
    {
        public static string ConvertToRawHtml(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }

        public static string ToUnSignString(string input)
        {
            input = input.Trim().ToLower();
            for (int i = 0x20; i < 0x30; i++)
            {
                input = input.Replace(((char)i).ToString(), " ");
            }
            input = input.Replace(".", "-");
            input = input.Replace(" ", "-");
            input = input.Replace(",", "-");
            input = input.Replace(";", "-");
            input = input.Replace(":", "-");
            input = input.Replace("  ", "-");
            Regex regex = new(@"\p{IsCombiningDiacriticalMarks}+");
            var str = input.Normalize(NormalizationForm.FormD);
            var str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            while (str2.IndexOf("?") >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }
            while (str2.Contains("--"))
            {
                str2 = str2.Replace("--", "-").ToLower();
            }
            return str2;
        }

        public static string RelativeDate(DateTime theDate)
        {
            var thresholds = new Dictionary<long, string>();
            const int minute = 60;
            const int hour = 60 * minute;
            const int day = 24 * hour;
            thresholds.Add(60, "{0} seconds ago");
            thresholds.Add(minute * 2, "a minute ago");
            thresholds.Add(45 * minute, "{0} minutes ago");
            thresholds.Add(120 * minute, "an hour ago");
            thresholds.Add(day, "{0} hours ago");
            thresholds.Add(day * 2, "yesterday");
            thresholds.Add(day * 30, "{0} days ago");
            thresholds.Add(day * 365, "{0} months ago");
            thresholds.Add(long.MaxValue, "{0} years ago");
            var since = (DateTime.Now.Ticks - theDate.Ticks) / 10000000;

            foreach (var threshold in thresholds.Keys)
            {
                if (since >= threshold) continue;
                var t = new TimeSpan((DateTime.Now.Ticks - theDate.Ticks));
                return string.Format(thresholds[threshold], (t.Days > 365 ? t.Days / 365 : (t.Days > 0 ? t.Days : (t.Hours > 0 ? t.Hours : (t.Minutes > 0 ? t.Minutes : (t.Seconds > 0 ? t.Seconds : 0))))).ToString());
            }

            return "";
        }

        public static bool ValidateJson(string s)
        {
            try
            {
                JToken.Parse(s);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static T ConvertIntoObject<T>(string json)
        {
            return ValidateJson(json) ? JsonConvert.DeserializeObject<T>(json) : default(T);
        }
    }
}