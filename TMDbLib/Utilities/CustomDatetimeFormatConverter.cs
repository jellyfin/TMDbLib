using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TMDbLib.Utilities
{
    public class CustomDatetimeFormatConverter : DateTimeConverterBase
    {
        public CultureInfo CultureInfo { get; set; }
        public string DatetimeFormat { get; set; }

        public CustomDatetimeFormatConverter()
        {
            CultureInfo = CultureInfo.GetCultureInfo("en-US");
            DatetimeFormat = "yyyy-MM-dd HH:mm:ss UTC";
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return DateTime.ParseExact(reader.Value.ToString(), DatetimeFormat, CultureInfo.CurrentCulture);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((DateTime)value).ToString(DatetimeFormat, CultureInfo));
        }
    }
}