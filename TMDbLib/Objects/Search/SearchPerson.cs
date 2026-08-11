using System.Collections.Generic;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General.Schema;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.Search;

/// <summary>
/// Person search result. Person-summary fields are inherited from <see cref="TmdbPersonSummary"/>;
/// the only extra field unique to /search/person is <c>known_for</c>.
/// </summary>
public class SearchPerson : TmdbPersonSummary
{
    /// <summary>
    /// Gets or sets the movies and TV shows the person is known for.
    /// </summary>
    [JsonPropertyName("known_for")]
    [JsonConverter(typeof(KnownForConverter))]
    public List<TmdbMediaSummary>? KnownFor { get; set; }
}
