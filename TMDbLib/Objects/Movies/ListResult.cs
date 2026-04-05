using System.Text.Json.Serialization;
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
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the favorite count.
    /// </summary>
    [JsonPropertyName("favorite_count")]
    public int FavoriteCount { get; set; }

    /// <summary>
    /// Gets or sets the list ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or sets a language code, e.g. en.
    /// </summary>
    [JsonPropertyName("iso_639_1")]
    public string? Iso_639_1 { get; set; }

    /// <summary>
    /// Gets or sets the item count in the list.
    /// </summary>
    [JsonPropertyName("item_Count")]
    public int ItemCount { get; set; }

    /// <summary>
    /// Gets or sets the list media type.
    /// </summary>
    [JsonPropertyName("list_type")]
    public MediaType ListType { get; set; }

    /// <summary>
    /// Gets or sets the list name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the poster image path.
    /// </summary>
    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }
}
