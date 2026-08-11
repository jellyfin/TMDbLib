using System.Text.Json.Serialization;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents a search container that includes a date range.
/// </summary>
/// <typeparam name="T">The type of items contained in the search results.</typeparam>
public class SearchContainerWithDates<T> : SearchContainer<T>
{
    /// <summary>
    /// Gets or sets the date range for the search results.
    /// </summary>
    [JsonPropertyName("dates")]
    public DateRange? Dates { get; set; }
}
