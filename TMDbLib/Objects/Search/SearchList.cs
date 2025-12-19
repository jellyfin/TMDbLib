using Newtonsoft.Json;

namespace TMDbLib.Objects.Search;

/// <summary>
/// Represents a list search result.
/// </summary>
public class SearchList
{
    /// <summary>
    /// Gets or sets the description of the list.
    /// </summary>
    [JsonProperty("description")]
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the number of times the list has been favorited.
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
    /// Gets or sets the number of items in the list.
    /// </summary>
    [JsonProperty("item_count")]
    public int ItemCount { get; set; }

    /// <summary>
    /// Gets or sets the type of the list.
    /// </summary>
    [JsonProperty("list_type")]
    public string ListType { get; set; }

    /// <summary>
    /// Gets or sets the name of the list.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the poster image path.
    /// </summary>
    [JsonProperty("poster_path")]
    public string PosterPath { get; set; }
}
