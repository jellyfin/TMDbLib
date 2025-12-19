using Newtonsoft.Json;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Movies;

/// <summary>
/// Represents a list result containing a movie.
/// </summary>
public class ListResult
{
    /// <summary>
    /// Gets or sets the list description.
    /// </summary>
    [JsonProperty("description")]
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the favorite count.
    /// </summary>
    [JsonProperty("favorite_count")]
    public int FavoriteCount { get; set; }

    /// <summary>
    /// Gets or sets the list ID.
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets a language code, e.g. en.
    /// </summary>
    [JsonProperty("iso_639_1")]
    public string Iso_639_1 { get; set; }

    /// <summary>
    /// Gets or sets the item count in the list.
    /// </summary>
    [JsonProperty("item_Count")]
    public int ItemCount { get; set; }

    /// <summary>
    /// Gets or sets the list media type.
    /// </summary>
    [JsonProperty("list_type")]
    public MediaType ListType { get; set; }

    /// <summary>
    /// Gets or sets the list name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the poster image path.
    /// </summary>
    [JsonProperty("poster_path")]
    public string PosterPath { get; set; }
}
