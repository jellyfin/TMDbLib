namespace TMDbLib.Objects.General.Schema;

/// <summary>
/// TV-specific credit addition (notably <c>episode_count</c>).
/// </summary>
public interface ITvCreditExtras
{
    /// <summary>
    /// Gets or sets the number of episodes the person worked on.
    /// </summary>
    int EpisodeCount { get; set; }
}
