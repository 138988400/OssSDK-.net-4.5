using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oss.Utilities
{
    internal static class DateUtils
    {
        private const string _iso8601DateFormat = "yyyy-MM-dd'T'HH:mm:ss.fff'Z'";
        private const string _rfc822DateFormat = @"ddd, dd MMM yyyy HH:mm:ss \G\M\T";

        public static string FormatIso8601Date(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.CreateSpecificCulture("en-US"));
        }

        public static string FormatRfc822Date(DateTime dt)
        {
            return dt.ToString(@"ddd, dd MMM yyyy HH:mm:ss \G\M\T", CultureInfo.InvariantCulture);
        }

        public static DateTime ParseRfc822Date(string dt)
        {
            return DateTime.SpecifyKind(DateTime.ParseExact(dt, @"ddd, dd MMM yyyy HH:mm:ss \G\M\T", CultureInfo.InvariantCulture), DateTimeKind.Utc);
        }
    }
}
