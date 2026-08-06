using System;
using System.Text.Json.Serialization;
using TMDbLib.Utilities;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.Collections;

/// <summary>
/// Specifies additional methods to include when retrieving collection information.
/// </summary>
[JsonConverter(typeof(TolerantEnumConverter<CollectionMethods>))]
[Flags]
public enum CollectionMethods
{
    /// <summary>
    /// No additional methods specified.
    /// </summary>
    [EnumValue("Undefined")]
    Undefined = 0,

    /// <summary>
    /// Include images for the collection.
    /// </summary>
    [EnumValue("images")]
    Images = 1,

    /// <summary>
    /// Include translations for the collection.
    /// </summary>
    [EnumValue("translations")]
    Translations = 2,
}
