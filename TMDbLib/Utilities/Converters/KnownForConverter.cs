using System;
using Newtonsoft.Json.Linq;
using TMDbLib.Objects.General;
using TMDbLib.Objects.General.Schema;

namespace TMDbLib.Utilities.Converters;

internal class KnownForConverter : JsonCreationConverter<TmdbMediaSummary>
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(TmdbMediaSummary);
    }

    protected override TmdbMediaSummary? GetInstance(JObject jObject)
    {
        var mediaType = jObject["media_type"]?.ToObject<MediaType>();

        switch (mediaType)
        {
            case MediaType.Movie:
                return new TmdbMovieSummary();
            case MediaType.Tv:
                return new TmdbTvSummary();
            case null:
                return null;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
