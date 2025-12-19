using TMDbLib.Utilities;

namespace TMDbLib.Objects.People;

/// <summary>
/// Represents the gender of a person.
/// </summary>
public enum PersonGender
{
    /// <summary>
    /// Unknown or not specified.
    /// </summary>
    [EnumValue(null)]
    Unknown = 0,

    /// <summary>
    /// Female.
    /// </summary>
    Female = 1,

    /// <summary>
    /// Male.
    /// </summary>
    Male = 2,

    /// <summary>
    /// Non-binary.
    /// </summary>
    NonBinary = 3
}
