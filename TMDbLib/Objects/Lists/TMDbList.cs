using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Lists;

/// <summary>
/// Represents the base class for TMDb lists.
/// </summary>
/// <typeparam name="TId">The type of the list ID.</typeparam>
public abstract class TMDbList<TId>
{
    /// <summary>
    /// Gets or sets the description of the list.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the number of times the list has been favorited.
    /// </summary>
    [JsonPropertyName("favorite_count")]
    public int FavoriteCount { get; set; }

    /// <summary>
    /// Gets or sets the list ID.
    /// </summary>
    [JsonPropertyName("id")]
    public TId? Id { get; set; }

    /// <summary>
    /// Gets or sets a language code, e.g. en.
    /// </summary>
    [JsonPropertyName("iso_639_1")]
    public string? Iso_639_1 { get; set; }

    /// <summary>
    /// Gets or sets the number of items in the list.
    /// </summary>
    [JsonPropertyName("item_count")]
    public int ItemCount { get; set; }

    /// <summary>
    /// Gets or sets the name of the list.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the poster path for the list.
    /// </summary>
    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }
}
