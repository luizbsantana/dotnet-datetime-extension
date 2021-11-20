using System;
using System.Collections.Generic;
using System.Linq;
using Brazil.Datetime.Models;
using Nager.Date;

namespace Brazil.Datetime
{
    /// <summary>
    /// Brazil DateTime extension class
    /// </summary>
    public static class BrazilDateTime
    {
        /// <summary>
        /// Convert a <see cref="DateTime"/> object to a new one with brazilian timezone
        /// </summary>
        /// <param name="date">DateTime object that will be converted</param>
        /// <returns>Converted DateTime with brazilian timezone</returns>
        public static DateTime ToBrazilianTimeZone(this DateTime date)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(date.ToUniversalTime(),
                Environment.OSVersion.Platform == PlatformID.Unix
                ? TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo")
                : TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
        }

        /// <summary>
        /// Convert a <see cref="DateTimeOffset"/> object to a new one with brazilian timezone
        /// </summary>
        /// <param name="date">DateTime object that will be converted</param>
        /// <returns>Converted DateTime with brazilian timezone</returns>
        public static DateTimeOffset ToBrazilianTimeZone(this DateTimeOffset date)
        {
            return Environment.OSVersion.Platform == PlatformID.Unix ?
                TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date, "America/Sao_Paulo") :
                TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date, "E. South America Standard Time");
        }

        /// <summary>
        /// Get a <see cref="DateTime"/> object the time of the day in the brazilian timezone
        /// </summary>
        /// <param name="date">DateTime that will be extracted the time of the day</param>
        /// <returns>Time of the day with brazilian timezone</returns>
        public static TimeSpan BrazilianTimeOfDay(this DateTime date)
        {
            return date.ToBrazilianTimeZone().TimeOfDay;
        }

        /// <summary>
        /// Check if a <see cref="DateTime"/> object is in brazilian weekend
        /// </summary>
        /// <param name="date">DateTime that will be checked</param>
        /// <returns>True if is weekend or False if it is not</returns>
        public static bool IsBrazilianWeekend(this DateTime date)
        {
            return DateSystem.IsWeekend(date, CountryCode.BR);
        }

        /// <summary>
        /// Get all brazilian holidays in the current year from a <see cref="DateTime"/> object
        /// </summary>
        /// <returns>A list with all brazilian holidays in the current year</returns>
        public static IEnumerable<Holiday> GetBrazilianHolidaysFromNow(this DateTime date)
        {
            return DateSystem.GetPublicHolidays(DateTime.UtcNow.ToBrazilianTimeZone().Year, CountryCode.BR)
                .Where(holiday => holiday.Date > date.Date)
                .Select(holiday => new Holiday(holiday.Date, holiday.Name));
        }

        /// <summary>
        /// Check if a <see cref="DateTime"/> object is a brazilian holiday
        /// </summary>
        /// <param name="date">DateTime that will be checked</param>
        /// <returns>True if is holiday or False if it is not</returns>
        public static bool IsBrazilianHoliday(this DateTime date)
        {
            return date.GetBrazilianHolidaysFromNow().Any(holiday => holiday.Date.Equals(date.Date));
        }

        /// <summary>
        /// Check if a <see cref="DateTime"/> object is a brazilian business day
        /// </summary>
        /// <param name="date">DateTime that will be checked</param>
        /// <returns>True if is business day or False if it is not</returns>
        public static bool IsBusinessDay(this DateTime date)
        {
            if (date.IsBrazilianHoliday())
                return false;
            if (date.IsBrazilianWeekend())
                return false;

            return true;
        }

        /// <summary>
        /// Get the next brazilian business day based in a <see cref="DateTime"/> object
        /// </summary>
        /// <param name="date">DateTime that the next business day will be based on</param>
        /// <returns>A DateTime corresponding the next business day in Brazil</returns>
        public static DateTime NextBusinessDay(this DateTime date)
        {
            date = date.ToBrazilianTimeZone().Date;

            if (date.IsBrazilianWeekend())
                return NextBusinessDay(date.AddDays(1));

            if (date.IsBrazilianHoliday())
                return NextBusinessDay(date.AddDays(1));

            return date;
        }
    }
}
