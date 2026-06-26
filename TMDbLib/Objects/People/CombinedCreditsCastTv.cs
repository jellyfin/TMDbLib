using Newtonsoft.Json;
using TMDbLib.Objects.General.Schema;

namespace TMDbLib.Objects.People;

/// <summary>
/// TV cast credit for a person.
/// </summary>
public class CombinedCreditsCastTv : TmdbTvSummary, ICastCredit, ITvCreditExtras
{
    /// <summary>
    /// Gets or sets the character played.
    /// </summary>
    [JsonProperty("character")]
    public string? Character { get; set; }

    /// <summary>
    /// Gets or sets the credit id.
    /// </summary>
    [JsonProperty("credit_id")]
    public string? CreditId { get; set; }

    /// <summary>
    /// Gets or sets the number of episodes the person appeared in.
    /// </summary>
    [JsonProperty("episode_count")]
    public int EpisodeCount { get; set; }

    /// <summary>
    /// Gets or sets the order in the cast list.
    /// </summary>
    [JsonProperty("order")]
    public int? Order { get; set; }
}
