using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TMDbLib.Utilities.Converters
{
    internal abstract class JsonCreationConverter<T> : JsonConverter
    {
        protected abstract T GetInstance(JObject jObject);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);

            T target = GetInstance(jObject);
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}