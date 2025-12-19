using Newtonsoft.Json;

namespace TMDbLib.Objects.Changes;

/// <summary>
/// Represents a change item for a destroyed action.
/// </summary>
public class ChangeItemDestroyed : ChangeItemBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeItemDestroyed"/> class.
    /// </summary>
    public ChangeItemDestroyed()
    {
        Action = ChangeAction.Destroyed;
    }

    /// <summary>
    /// Gets or sets the value that was destroyed.
    /// </summary>
    [JsonProperty("value")]
    public object Value { get; set; }
}
