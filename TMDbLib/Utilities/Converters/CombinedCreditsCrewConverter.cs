using System;
using Newtonsoft.Json.Linq;
using TMDbLib.Objects.General;
using TMDbLib.Objects.General.Schema;
using TMDbLib.Objects.People;

namespace TMDbLib.Utilities.Converters;

internal class CombinedCreditsCrewConverter : JsonCreationConverter<TmdbMediaSummary>
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
                return new CombinedCreditsCrewMovie();
            case MediaType.Tv:
                return new CombinedCreditsCrewTv();
            case null:
                return null;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
