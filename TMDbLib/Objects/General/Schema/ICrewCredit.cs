namespace TMDbLib.Objects.General.Schema;

/// <summary>
/// Crew-credit annotation (department, job, credit id) attached on top of a media
/// or person summary.
/// </summary>
public interface ICrewCredit
{
    /// <summary>
    /// Gets or sets the credit identifier.
    /// </summary>
    string? CreditId { get; set; }

    /// <summary>
    /// Gets or sets the crew department.
    /// </summary>
    string? Department { get; set; }

    /// <summary>
    /// Gets or sets the specific job title.
    /// </summary>
    string? Job { get; set; }
}
