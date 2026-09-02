using System.Collections.Generic;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General;
using TMDbLib.Objects.General.Schema;

namespace TMDbLib.Objects.Lists;

/// <summary>
/// Represents a generic TMDb list with search results.
/// </summary>
public class GenericList : TMDbList, IPagedResult<TmdbEntity>
{
    /// <summary>
    /// Gets or sets the username of the list creator.
    /// </summary>
    [JsonPropertyName("created_by")]
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the list of items in the list.
    /// </summary>
    [JsonPropertyName("items")]
    public List<TmdbEntity>? Items { get; set; }

    /// <summary>
    /// Gets or sets the page of items this response holds. TMDb serves the items of a list in pages
    /// of 20, so a list with a higher <see cref="TMDbList.ItemCount"/> needs more than one request.
    /// </summary>
    [JsonPropertyName("page")]
    public int Page { get; set; }

    /// <summary>
    /// Gets or sets the total number of pages the items of the list are split into.
    /// </summary>
    [JsonPropertyName("total_pages")]
    public int TotalPages { get; set; }

    /// <summary>
    /// Gets or sets the total number of items across all pages.
    /// </summary>
    [JsonPropertyName("total_results")]
    public int TotalResults { get; set; }

    /// <inheritdoc />
    IReadOnlyList<TmdbEntity>? IPagedResult<TmdbEntity>.Items => Items;
}
