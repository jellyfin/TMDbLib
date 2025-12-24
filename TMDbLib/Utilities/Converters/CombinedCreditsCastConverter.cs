using System;
using Newtonsoft.Json.Linq;
using TMDbLib.Objects.General;
using TMDbLib.Objects.People;

namespace TMDbLib.Utilities.Converters;

internal class CombinedCreditsCastConverter : JsonCreationConverter<CombinedCreditsCastBase>
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(CombinedCreditsCastBase);
    }

    protected override CombinedCreditsCastBase GetInstance(JObject jObject)
    {
        MediaType mediaType = jObject["media_type"].ToObject<MediaType>();

        switch (mediaType)
        {
            case MediaType.Movie:
                return new CombinedCreditsCastMovie();
            case MediaType.Tv:
                return new CombinedCreditsCastTv();
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
