using System;
using Newtonsoft.Json;

namespace TMDbLib.Utilities.Converters
{
    internal class EnumStringValueConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string str = EnumMemberCache.GetString(value);

            writer.WriteValue(str);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object val = EnumMemberCache.GetValue(reader.Value as string, objectType);

            return val;
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}
