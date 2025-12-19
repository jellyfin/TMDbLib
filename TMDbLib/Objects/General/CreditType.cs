using TMDbLib.Utilities;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents the type of credit.
/// </summary>
public enum CreditType
{
    /// <summary>
    /// Unknown credit type.
    /// </summary>
    Unknown,

    /// <summary>
    /// Crew member credit.
    /// </summary>
    [EnumValue("crew")]
    Crew = 1,

    /// <summary>
    /// Cast member credit.
    /// </summary>
    [EnumValue("cast")]
    Cast = 2
}
