using System;
using System.Globalization;
using Newtonsoft.Json;

namespace TMDbLib.Utilities.Converters
{
    public class TmdbNullIntAsZero : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(int);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value is null)
            {
                return 0;
            }

            return Convert.ToInt32(reader.Value, CultureInfo.InvariantCulture);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value?.ToString());
        }
    }
}
