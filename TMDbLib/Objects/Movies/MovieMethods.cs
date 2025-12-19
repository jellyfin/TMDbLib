using System;
using TMDbLib.Utilities;

namespace TMDbLib.Objects.Movies;

/// <summary>
/// Specifies additional movie data to retrieve from the API.
/// </summary>
[Flags]
public enum MovieMethods
{
    /// <summary>
    /// Undefined or no additional data.
    /// </summary>
    [EnumValue("Undefined")]
    Undefined = 0,

    /// <summary>
    /// Include alternative titles.
    /// </summary>
    [EnumValue("alternative_titles")]
    AlternativeTitles = 1 << 0,

    /// <summary>
    /// Include cast and crew credits.
    /// </summary>
    [EnumValue("credits")]
    Credits = 1 << 1,

    /// <summary>
    /// Include images.
    /// </summary>
    [EnumValue("images")]
    Images = 1 << 2,

    /// <summary>
    /// Include keywords.
    /// </summary>
    [EnumValue("keywords")]
    Keywords = 1 << 3,

    /// <summary>
    /// Include releases information.
    /// </summary>
    [EnumValue("releases")]
    Releases = 1 << 4,

    /// <summary>
    /// Include videos.
    /// </summary>
    [EnumValue("videos")]
    Videos = 1 << 5,

    /// <summary>
    /// Include translations.
    /// </summary>
    [EnumValue("translations")]
    Translations = 1 << 6,

    /// <summary>
    /// Include similar movies.
    /// </summary>
    [EnumValue("similar")]
    Similar = 1 << 7,

    /// <summary>
    /// Include user reviews.
    /// </summary>
    [EnumValue("reviews")]
    Reviews = 1 << 8,

    /// <summary>
    /// Include lists containing this movie.
    /// </summary>
    [EnumValue("lists")]
    Lists = 1 << 9,

    /// <summary>
    /// Include change history.
    /// </summary>
    [EnumValue("changes")]
    Changes = 1 << 10,

    /// <summary>
    /// Requires a valid user session to be set on the client object.
    /// </summary>
    [EnumValue("account_states")]
    AccountStates = 1 << 11,

    /// <summary>
    /// Include release dates by country.
    /// </summary>
    [EnumValue("release_dates")]
    ReleaseDates = 1 << 12,

    /// <summary>
    /// Include recommended movies.
    /// </summary>
    [EnumValue("recommendations")]
    Recommendations = 1 << 13,

    /// <summary>
    /// Include external IDs.
    /// </summary>
    [EnumValue("external_ids")]
    ExternalIds = 1 << 14,

    /// <summary>
    /// Include watch provider information.
    /// </summary>
    [EnumValue("watch/providers")]
    WatchProviders = 1 << 15
}
