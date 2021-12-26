using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Application.Commons.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime FirstDayOfWeek(this DateTime dt)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            var diff = dt.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;

            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-diff).Date;
        }

        public static DateTime LastDayOfWeek(this DateTime dateTime) => dateTime.FirstDayOfWeek().AddDays(6);

        public static DateTime NextNumberOfWeek(this DateTime dateTime, int numberOfWeek)
        {
            int daysUntilSaturday = ((int)DayOfWeek.Saturday - (int)dateTime.DayOfWeek + 7) % 7;
            DateTime nextSaturday = dateTime.AddDays(daysUntilSaturday);

            return nextSaturday.AddDays(7 * numberOfWeek);
        }

        public static DateTime FirstDayOfMonth(this DateTime dateTime) => new DateTime(dateTime.Year, dateTime.Month, 1);

        public static DateTime LastDayOfMonth(this DateTime dateTime) => dateTime.FirstDayOfMonth().AddMonths(1).AddDays(-1);

        public static DateTime FirstDayOfNextMonth(this DateTime dateTime) => dateTime.FirstDayOfMonth().AddMonths(1);

        public static DateTime ConvertTimeFromUtc(this DateTime dateTime, string timeZoneId)
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZoneInfo);
        }

        public static IEnumerable<T> ConvertTimeFromUtc<T>(this IEnumerable<T> sources, string[] columnsTobeConverted, string timeZoneId)
        {
            var properties = typeof(T).GetProperties().Where(c => columnsTobeConverted.Contains(c.Name)).ToArray();
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

            return sources?.Select(item =>
            {
                properties.ForAll(property =>
                {
                    var value = property.GetValue(item);
                    if (value != null)
                    {
                        if (value is DateTime dt)
                        {
                            property.SetValue(item, TimeZoneInfo.ConvertTimeFromUtc(dt, timeZoneInfo));
                        }
                        if (Nullable.GetUnderlyingType(value.GetType()) == typeof(DateTime))
                        {
                            property.SetValue(item, TimeZoneInfo.ConvertTimeFromUtc(((DateTime?)value).Value, timeZoneInfo));
                        }
                    }
                });
                return item;
            });
        }

        public static DateTime EndOfDay(this DateTime date)
        {
            return date.Date.AddDays(1).AddTicks(-1);
        }

        public static DateTime StartOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }

        public static DateTime ConvertTimeToUtc(this DateTime dateTime, string timeZoneId)
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeToUtc(dateTime, timeZoneInfo);
        }

        public static DateTime GetNextWeekday(DateTime start, int day)
        {
            int daysToAdd = (day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }

        public static string ConvertDateTimeFormatByDate(this DateTime dt, string format)
        {
            return dt.ToString(format);
        }

        public static string ConvertDateTimeToStringFormat(this DateTime dt, string format)
        {
            if (DateTime.TryParse(dt.ToString(CultureInfo.InvariantCulture), out DateTime outputDate))
                return outputDate.ToString(format);
            return string.Empty;
        }

        public static int CalculateAge(this DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if (dob.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}