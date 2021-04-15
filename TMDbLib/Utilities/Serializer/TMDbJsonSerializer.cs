using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Utilities.Serializer
{
    public class TMDbJsonSerializer : ITMDbSerializer
    {
        private readonly JsonSerializer _serializer;
        private readonly Encoding _encoding = new UTF8Encoding(false);

        public static TMDbJsonSerializer Instance { get; } = new();

        public TMDbJsonSerializer()
        {
            _serializer = JsonSerializer.CreateDefault();
            _serializer.Converters.Add(new ChangeItemConverter());
            _serializer.Converters.Add(new AccountStateConverter());
            _serializer.Converters.Add(new KnownForConverter());
            _serializer.Converters.Add(new SearchBaseConverter());
            _serializer.Converters.Add(new TaggedImageConverter());
            _serializer.Converters.Add(new TolerantEnumConverter());
        }

        public void Serialize(Stream target, object obj, Type type)
        {
            using var sw = new StreamWriter(target, _encoding, 4096, true);
            using var jw = new JsonTextWriter(sw);

            _serializer.Serialize(jw, obj, type);
        }

        public object Deserialize(Stream source, Type type)
        {
            using var sr = new StreamReader(source, _encoding, false, 4096, true);
            using var jr = new JsonTextReader(sr);

            return _serializer.Deserialize(jr, type);
        }
    }
}