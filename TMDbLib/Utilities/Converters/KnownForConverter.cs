using System;
using Newtonsoft.Json.Linq;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace TMDbLib.Utilities.Converters;

internal class KnownForConverter : JsonCreationConverter<KnownForBase>
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(KnownForBase);
    }

    protected override KnownForBase? GetInstance(JObject jObject)
    {
        var mediaType = jObject["media_type"]?.ToObject<MediaType>();

        switch (mediaType)
        {
            case MediaType.Movie:
                return new KnownForMovie();
            case MediaType.Tv:
                return new KnownForTv();
            case null:
                return null;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
