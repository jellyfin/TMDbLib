using System.Text.Json.Serialization;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents external IDs for a media item.
/// </summary>
[JsonDerivedType(typeof(ExternalIdsMovie))]
[JsonDerivedType(typeof(ExternalIdsTvShow))]
[JsonDerivedType(typeof(ExternalIdsTvSeason))]
[JsonDerivedType(typeof(ExternalIdsTvEpisode))]
[JsonDerivedType(typeof(ExternalIdsPerson))]
public class ExternalIds
{
    /// <summary>
    /// Gets or sets the Freebase ID.
    /// </summary>
    [JsonPropertyName("freebase_id")]
    public string? FreebaseId { get; set; }

    /// <summary>
    /// Gets or sets the Freebase MID.
    /// </summary>
    [JsonPropertyName("freebase_mid")]
    public string? FreebaseMid { get; set; }

    /// <summary>
    /// Gets or sets the TMDb ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the TVRage ID.
    /// </summary>
    [JsonPropertyName("tvrage_id")]
    public int? TvrageId { get; set; }

    /// <summary>
    /// Gets or sets the Wikidata ID.
    /// </summary>
    [JsonPropertyName("wikidata_id")]
    public string? WikidataId { get; set; }
}
