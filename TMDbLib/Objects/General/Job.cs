using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents a department and its associated job titles.
/// </summary>
public class Job
{
    /// <summary>
    /// Gets or sets the department name.
    /// </summary>
    [JsonPropertyName("department")]
    public string? Department { get; set; }

    /// <summary>
    /// Gets or sets the list of job titles in this department.
    /// </summary>
    [JsonPropertyName("jobs")]
    public List<string>? Jobs { get; set; }
}
