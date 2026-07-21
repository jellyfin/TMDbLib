using System.Collections.Generic;
using Newtonsoft.Json;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// TV episode with full details.
/// </summary>
public class TvEpisode : TvEpisodeBase
{
    /// <summary>
    /// Gets or sets the account states.
    /// </summary>
    [JsonProperty("account_states")]
    public TvAccountState? AccountStates { get; set; }

    /// <summary>
    /// Gets or sets the credits including guest stars.
    /// </summary>
    [JsonProperty("credits")]
    public CreditsWithGuestStars? Credits { get; set; }

    /// <summary>
    /// Gets or sets the crew members.
    /// </summary>
    [JsonProperty("crew")]
    public List<Crew>? Crew { get; set; }

    /// <summary>
    /// Gets or sets the external ids.
    /// </summary>
    [JsonProperty("external_ids")]
    public ExternalIdsTvEpisode? ExternalIds { get; set; }

    /// <summary>
    /// Gets or sets the guest stars.
    /// </summary>
    [JsonProperty("guest_stars")]
    public List<Cast>? GuestStars { get; set; }

    /// <summary>
    /// Gets or sets the still images.
    /// </summary>
    [JsonProperty("images")]
    public StillImages? Images { get; set; }

    /// <summary>
    /// Gets or sets the videos.
    /// </summary>
    [JsonProperty("videos")]
    public ResultContainer<Video>? Videos { get; set; }

    /// <summary>
    /// Gets or sets the translations.
    /// </summary>
    [JsonProperty("translations")]
    public TranslationsContainer? Translations { get; set; }
}
