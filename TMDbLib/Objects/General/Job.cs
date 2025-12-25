using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents a department and its associated job titles.
/// </summary>
public class Job
{
    /// <summary>
    /// Gets or sets the department name.
    /// </summary>
    [JsonProperty("department")]
    public string? Department { get; set; }

    /// <summary>
    /// Gets or sets the list of job titles in this department.
    /// </summary>
    [JsonProperty("jobs")]
    public List<string>? Jobs { get; set; }
}
