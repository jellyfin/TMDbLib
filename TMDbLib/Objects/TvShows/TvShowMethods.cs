using System;
using TMDbLib.Utilities;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Defines the optional data that can be retrieved along with TV show details.
/// </summary>
[Flags]
public enum TvShowMethods
{
    /// <summary>
    /// No additional data.
    /// </summary>
    [EnumValue("Undefined")]
    Undefined = 0,

    /// <summary>
    /// Include credits information.
    /// </summary>
    [EnumValue("credits")]
    Credits = 1 << 0,

    /// <summary>
    /// Include images.
    /// </summary>
    [EnumValue("images")]
    Images = 1 << 1,

    /// <summary>
    /// Include external IDs.
    /// </summary>
    [EnumValue("external_ids")]
    ExternalIds = 1 << 2,

    /// <summary>
    /// Include content ratings.
    /// </summary>
    [EnumValue("content_ratings")]
    ContentRatings = 1 << 3,

    /// <summary>
    /// Include alternative titles.
    /// </summary>
    [EnumValue("alternative_titles")]
    AlternativeTitles = 1 << 4,

    /// <summary>
    /// Include keywords.
    /// </summary>
    [EnumValue("keywords")]
    Keywords = 1 << 5,

    /// <summary>
    /// Include similar TV shows.
    /// </summary>
    [EnumValue("similar")]
    Similar = 1 << 6,

    /// <summary>
    /// Include videos.
    /// </summary>
    [EnumValue("videos")]
    Videos = 1 << 7,

    /// <summary>
    /// Include translations.
    /// </summary>
    [EnumValue("translations")]
    Translations = 1 << 8,

    /// <summary>
    /// Include account states.
    /// </summary>
    [EnumValue("account_states")]
    AccountStates = 1 << 9,

    /// <summary>
    /// Include changes.
    /// </summary>
    [EnumValue("changes")]
    Changes = 1 << 10,

    /// <summary>
    /// Include recommendations.
    /// </summary>
    [EnumValue("recommendations")]
    Recommendations = 1 << 11,

    /// <summary>
    /// Include reviews.
    /// </summary>
    [EnumValue("reviews")]
    Reviews = 1 << 12,

    /// <summary>
    /// Include watch providers.
    /// </summary>
    [EnumValue("watch/providers")]
    WatchProviders = 1 << 13,

    /// <summary>
    /// Include episode groups.
    /// </summary>
    [EnumValue("episode_groups")]
    EpisodeGroups = 1 << 14,

    /// <summary>
    /// Include aggregated credits.
    /// </summary>
    [EnumValue("aggregate_credits")]
    CreditsAggregate = 1 << 15,
}
