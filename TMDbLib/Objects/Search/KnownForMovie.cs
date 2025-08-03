using System;
using Newtonsoft.Json;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Search;

public class KnownForMovie : KnownForBase
{
    public KnownForMovie()
    {
        MediaType = MediaType.Movie;
    }

    [JsonProperty("adult")]
    public bool Adult { get; set; }

    [JsonProperty("original_title")]
    public string OriginalTitle { get; set; }

    [JsonProperty("release_date")]
    public DateTime? ReleaseDate { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("video")]
    public bool Vide { get; set; }
}
