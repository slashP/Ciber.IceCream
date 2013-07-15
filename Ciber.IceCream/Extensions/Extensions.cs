using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CiberIs.Extensions
{
    public static class Extensions
    {
        public static DateTime EuropeanTime(this DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime,
                                                   TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));
        }

        public static void Each<T>(this IEnumerable<T> ie, Action<T, int> action)
        {
            var i = 0;
            foreach (var e in ie) action(e, i++);
        }

        public static int ToInt(this double number)
        {
            return (int)Math.Round(number, 0);
        }

        public static int SafeValue(this int? number)
        {
            return number.HasValue ? number.Value : 0;
        }
    }
}