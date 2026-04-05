using System.Collections.Generic;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Represents a TV episode with full details.
/// </summary>
public class TvEpisode : TvEpisodeBase
{
    /// <summary>
    /// Gets or sets the account states for the episode.
    /// </summary>
    [JsonPropertyName("account_states")]
    public TvAccountState? AccountStates { get; set; }

    /// <summary>
    /// Gets or sets the credits including guest stars for the episode.
    /// </summary>
    [JsonPropertyName("credits")]
    public CreditsWithGuestStars? Credits { get; set; }

    /// <summary>
    /// Gets or sets the list of crew members for the episode.
    /// </summary>
    [JsonPropertyName("crew")]
    public List<Crew>? Crew { get; set; }

    /// <summary>
    /// Gets or sets the external IDs for the episode.
    /// </summary>
    [JsonPropertyName("external_ids")]
    public ExternalIdsTvEpisode? ExternalIds { get; set; }

    /// <summary>
    /// Gets or sets the list of guest stars for the episode.
    /// </summary>
    [JsonPropertyName("guest_stars")]
    public List<Cast>? GuestStars { get; set; }

    /// <summary>
    /// Gets or sets the still images for the episode.
    /// </summary>
    [JsonPropertyName("images")]
    public StillImages? Images { get; set; }

    /// <summary>
    /// Gets or sets the videos for the episode.
    /// </summary>
    [JsonPropertyName("videos")]
    public ResultContainer<Video>? Videos { get; set; }

    /// <summary>
    /// Gets or sets the translations for the episode.
    /// </summary>
    [JsonPropertyName("translations")]
    public TranslationsContainer? Translations { get; set; }
}
