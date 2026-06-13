using System.Collections.Generic;
using System.Text.Json.Serialization;
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
    [JsonPropertyName("account_states")]
    public TvAccountState? AccountStates { get; set; }

    /// <summary>
    /// Gets or sets the credits including guest stars.
    /// </summary>
    [JsonPropertyName("credits")]
    public CreditsWithGuestStars? Credits { get; set; }

    /// <summary>
    /// Gets or sets the crew members.
    /// </summary>
    [JsonPropertyName("crew")]
    public List<Crew>? Crew { get; set; }

    /// <summary>
    /// Gets or sets the external ids.
    /// </summary>
    [JsonPropertyName("external_ids")]
    public ExternalIdsTvEpisode? ExternalIds { get; set; }

    /// <summary>
    /// Gets or sets the guest stars.
    /// </summary>
    [JsonPropertyName("guest_stars")]
    public List<Cast>? GuestStars { get; set; }

    /// <summary>
    /// Gets or sets the still images.
    /// </summary>
    [JsonPropertyName("images")]
    public StillImages? Images { get; set; }

    /// <summary>
    /// Gets or sets the videos.
    /// </summary>
    [JsonPropertyName("videos")]
    public ResultContainer<Video>? Videos { get; set; }

    /// <summary>
    /// Gets or sets the translations.
    /// </summary>
    [JsonPropertyName("translations")]
    public TranslationsContainer? Translations { get; set; }
}
