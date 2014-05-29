using System.Globalization;
using Newtonsoft.Json.Converters;

namespace TMDbLib.Converters
{
    class DateTimeConverterYearMonthDay : IsoDateTimeConverter
    {
        public DateTimeConverterYearMonthDay()
        {
            DateTimeFormat = "yyyy-MM-dd";
        }
    }

    class DateTimeConverterYearMonthDayHourMinuteSecondUtc : IsoDateTimeConverter
    {
        public DateTimeConverterYearMonthDayHourMinuteSecondUtc()
        {
            DateTimeFormat = "yyyy-MM-dd HH:mm:ss UTC";
            DateTimeStyles = DateTimeStyles.AssumeUniversal;
        }
    }
}
