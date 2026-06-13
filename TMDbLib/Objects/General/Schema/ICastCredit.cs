namespace TMDbLib.Objects.General.Schema;

/// <summary>
/// Cast-credit annotation (character, credit id, order) attached on top of a media
/// or person summary.
/// </summary>
public interface ICastCredit
{
    /// <summary>
    /// Gets or sets the character played.
    /// </summary>
    string? Character { get; set; }

    /// <summary>
    /// Gets or sets the credit identifier.
    /// </summary>
    string? CreditId { get; set; }

    /// <summary>
    /// Gets or sets the order in the cast list.
    /// </summary>
    int? Order { get; set; }
}
