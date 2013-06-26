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
    }
}