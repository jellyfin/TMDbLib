using System;
using TMDbLib.Utilities;

namespace TMDbLib.Objects.People;

/// <summary>
/// Specifies additional person data to retrieve from the API.
/// </summary>
[Flags]
public enum PersonMethods
{
    /// <summary>
    /// Undefined or no additional data.
    /// </summary>
    [EnumValue("Undefined")]
    Undefined = 0,

    /// <summary>
    /// Include movie credits.
    /// </summary>
    [EnumValue("movie_credits")]
    MovieCredits = 1,

    /// <summary>
    /// Include TV credits.
    /// </summary>
    [EnumValue("tv_credits")]
    TvCredits = 2,

    /// <summary>
    /// Include external IDs.
    /// </summary>
    [EnumValue("external_ids")]
    ExternalIds = 4,

    /// <summary>
    /// Include profile images.
    /// </summary>
    [EnumValue("images")]
    Images = 8,

    /// <summary>
    /// Include tagged images.
    /// </summary>
    [EnumValue("tagged_images")]
    TaggedImages = 16,

    /// <summary>
    /// Include change history.
    /// </summary>
    [EnumValue("changes")]
    Changes = 32,

    /// <summary>
    /// Include translations.
    /// </summary>
    [EnumValue("translations")]
    Translations = 64,
}
