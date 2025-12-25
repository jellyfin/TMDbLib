using Newtonsoft.Json;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents external IDs for a media item.
/// </summary>
public class ExternalIds
{
    /// <summary>
    /// Gets or sets the Freebase ID.
    /// </summary>
    [JsonProperty("freebase_id")]
    public string? FreebaseId { get; set; }

    /// <summary>
    /// Gets or sets the Freebase MID.
    /// </summary>
    [JsonProperty("freebase_mid")]
    public string? FreebaseMid { get; set; }

    /// <summary>
    /// Gets or sets the TMDb ID.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the TVRage ID.
    /// </summary>
    [JsonProperty("tvrage_id")]
    public string? TvrageId { get; set; }

    /// <summary>
    /// Gets or sets the Wikidata ID.
    /// </summary>
    [JsonProperty("wikidata_id")]
    public string? WikidataId { get; set; }
}
