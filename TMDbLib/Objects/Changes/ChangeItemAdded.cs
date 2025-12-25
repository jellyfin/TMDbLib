using Newtonsoft.Json;

namespace TMDbLib.Objects.Changes;

/// <summary>
/// Represents a change item for an added action.
/// </summary>
public class ChangeItemAdded : ChangeItemBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeItemAdded"/> class.
    /// </summary>
    public ChangeItemAdded()
    {
        Action = ChangeAction.Added;
    }

    /// <summary>
    /// Gets or sets the value that was added.
    /// </summary>
    [JsonProperty("value")]
    public object? Value { get; set; }
}
