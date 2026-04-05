using System.Text.Json.Serialization;

namespace TMDbLib.Objects.RestRequests;

public sealed class ListCreateIBody : IBody
{
    public ListCreateIBody(string name, string description, string? language = null)
    {
        Name = name;
        Description = description;
        Language = language;
    }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("language")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Language { get; set; }
}
