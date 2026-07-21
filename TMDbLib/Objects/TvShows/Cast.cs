using System.Text.Json.Serialization;
using TMDbLib.Objects.General.Schema;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Cast member on a TV show or episode.
/// </summary>
public class Cast : TmdbPersonSummary, ICastCredit
{
    /// <summary>
    /// Gets or sets the character played.
    /// </summary>
    [JsonPropertyName("character")]
    public string? Character { get; set; }

    /// <summary>
    /// Gets or sets the credit id.
    /// </summary>
    [JsonPropertyName("credit_id")]
    public string? CreditId { get; set; }

    /// <summary>
    /// Gets or sets the order in the cast list.
    /// </summary>
    [JsonPropertyName("order")]
    public int? Order { get; set; }
}
