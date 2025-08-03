using Newtonsoft.Json;

namespace TMDbLib.Objects.General;

public class TranslationData
{
    [JsonProperty("name")]
    public string Name { get; set; }

    // Private hack to ensure two properties (name, title) are deserialized into Name.
    // Tv Shows and Movies will use different names for their translation data.
    [JsonProperty("title")]
    private string Title
    {
        set => Name = value;
    }

    [JsonProperty("overview")]
    public string Overview { get; set; }

    // Private hack to ensure two properties (overview, biography) are deserialized into Overview.
    // Most of the entities have an overview, but people have a biography.
    [JsonProperty("biography")]
    private string Biography
    {
        set => Overview = value;
    }

    [JsonProperty("homepage")]
    public string HomePage { get; set; }

    [JsonProperty("tagline")]
    public string Tagline { get; set; }

    [JsonProperty("runtime")]
    public int Runtime { get; set; }
}
