using Newtonsoft.Json;

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
    [JsonProperty("dates")]
    public DateRange? Dates { get; set; }
}
