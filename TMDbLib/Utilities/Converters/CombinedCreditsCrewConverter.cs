using System;
using Newtonsoft.Json.Linq;
using TMDbLib.Objects.General;
using TMDbLib.Objects.People;

namespace TMDbLib.Utilities.Converters;

internal class CombinedCreditsCrewConverter : JsonCreationConverter<CombinedCreditsCrewBase>
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(CombinedCreditsCrewBase);
    }

    protected override CombinedCreditsCrewBase GetInstance(JObject jObject)
    {
        MediaType mediaType = jObject["media_type"].ToObject<MediaType>();

        switch (mediaType)
        {
            case MediaType.Movie:
                return new CombinedCreditsCrewMovie();
            case MediaType.Tv:
                return new CombinedCreditsCrewTv();
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
