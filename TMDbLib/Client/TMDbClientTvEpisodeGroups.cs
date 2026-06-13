using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.TvShows;
using TMDbLib.Rest;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    /// <summary>
    /// Gets a TV episode group collection by id.
    /// </summary>
    /// <param name="id">The episode group id.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The episode group collection.</returns>
    public async Task<TvGroupCollection?> GetTvEpisodeGroupsAsync(string id, string? language = null, CancellationToken cancellationToken = default)
    {
        var req = _client.Create("tv/episode_group/{id}");
        req.AddUrlSegment("id", id);

        language ??= DefaultLanguage;
        if (!string.IsNullOrWhiteSpace(language))
        {
            req.AddParameter("language", language);
        }

        using var response = await req.Get<TvGroupCollection>(cancellationToken).ConfigureAwait(false);

        if (!response.IsValid)
        {
            return null;
        }

        return await response.GetDataObject().ConfigureAwait(false);
    }
}
