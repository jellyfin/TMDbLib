using Newtonsoft.Json;

namespace TMDbLib.Objects.General.Schema;

/// <summary>
/// Root of every TMDb entity: id, popularity, and (when polymorphic) media-type discriminator.
/// </summary>
public class TmdbEntity
{
    /// <summary>
    /// Gets or sets the TMDb id.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the media-type discriminator for polymorphic responses
    /// (search/multi, known_for, trending, etc.).
    /// </summary>
    [JsonProperty("media_type")]
    public MediaType MediaType { get; set; }

    /// <summary>
    /// Gets or sets the TMDb popularity score.
    /// </summary>
    [JsonProperty("popularity")]
    public double Popularity { get; set; }
}
