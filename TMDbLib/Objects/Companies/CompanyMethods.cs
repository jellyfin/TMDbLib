using System;
using System.Text.Json.Serialization;
using TMDbLib.Utilities;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.Companies;

/// <summary>
/// Specifies additional methods to include when retrieving company information.
/// </summary>
[JsonConverter(typeof(TolerantEnumConverter<CompanyMethods>))]
[Flags]
public enum CompanyMethods
{
    /// <summary>
    /// No additional methods specified.
    /// </summary>
    [EnumValue("Undefined")]
    Undefined = 0,

    /// <summary>
    /// Include movies associated with the company.
    /// </summary>
    [EnumValue("movies")]
    Movies = 1,

    /// <summary>
    /// Include alternative names for the company.
    /// </summary>
    [EnumValue("alternative_names")]
    AlternativeNames = 2,

    /// <summary>
    /// Include logo images for the company.
    /// </summary>
    [EnumValue("images")]
    Images = 4,
}
