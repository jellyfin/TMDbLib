using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.Credit;

/// <summary>
/// Represents media information in a credit context, including TV show details.
/// </summary>
public class CreditMedia
{
    /// <summary>
    /// Gets or sets the character name for acting credits.
    /// </summary>
    [JsonProperty("character")]
    public string Character { get; set; }

    /// <summary>
    /// Gets or sets the list of episodes associated with this credit.
    /// </summary>
    [JsonProperty("episodes")]
    public List<CreditEpisode> Episodes { get; set; }

    /// <summary>
    /// Gets or sets the media item's unique identifier.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the media item.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the original name of the media item.
    /// </summary>
    [JsonProperty("original_name")]
    public string OriginalName { get; set; }

    /// <summary>
    /// Gets or sets the list of seasons associated with this credit.
    /// </summary>
    [JsonProperty("seasons")]
    public List<CreditSeason> Seasons { get; set; }
}
