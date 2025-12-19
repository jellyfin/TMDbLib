using Newtonsoft.Json;

namespace TMDbLib.Objects.Credit;

/// <summary>
/// Represents a person in a credit context.
/// </summary>
public class CreditPerson
{
    /// <summary>
    /// Gets or sets the person's unique identifier.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the person's name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }
}
