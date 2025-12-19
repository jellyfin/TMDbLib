using System;
using TMDbLib.Utilities;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Defines the optional data that can be retrieved along with TV season details.
/// </summary>
[Flags]
public enum TvSeasonMethods
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
    Credits = 1,

    /// <summary>
    /// Include images.
    /// </summary>
    [EnumValue("images")]
    Images = 2,

    /// <summary>
    /// Include external IDs.
    /// </summary>
    [EnumValue("external_ids")]
    ExternalIds = 4,

    /// <summary>
    /// Include videos.
    /// </summary>
    [EnumValue("videos")]
    Videos = 8,

    /// <summary>
    /// Include account states.
    /// </summary>
    [EnumValue("account_states")]
    AccountStates = 16,

    /// <summary>
    /// Include translations.
    /// </summary>
    [EnumValue("translations")]
    Translations = 32,
}
