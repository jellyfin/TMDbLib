using Newtonsoft.Json;
using TMDbLib.Utilities;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.General
{
    [JsonConverter(typeof(EnumStringValueConverter))]
    public enum MediaType
    {
        Unknown,

        [EnumValue("movie")]
        Movie = 1,

        [EnumValue("tv")]
        Tv = 2,

        [EnumValue("person")]
        Person = 3,

        [EnumValue("episode")]
        Episode = 4,

        [EnumValue("tv_episode")]
        TvEpisode = 5,

        [EnumValue("season")]
        Season = 6,

        [EnumValue("tv_season")]
        TvSeason = 7,
        
        [EnumValue("collection")]
        Collection = 8
    }
}