using System;
using System.Text.Json.Serialization;
using TMDbLib.Utilities;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Additional TV episode data to retrieve from the API.
/// </summary>
[JsonConverter(typeof(TolerantEnumConverter<TvEpisodeMethods>))]
[Flags]
public enum TvEpisodeMethods
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

    /// <summary>
    /// Include changes.
    /// </summary>
    [EnumValue("changes")]
    Changes = 64,
}
