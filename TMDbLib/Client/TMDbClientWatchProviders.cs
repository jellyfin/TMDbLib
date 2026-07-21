using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Rest;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    /// <summary>
    /// Gets the countries with watch provider (OTT/streaming) data.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The available regions.</returns>
    /// <remarks>Uses <see cref="DefaultLanguage"/> to translate data.</remarks>
    public async Task<ResultContainer<WatchProviderRegion>?> GetWatchProviderRegionsAsync(CancellationToken cancellationToken = default)
    {
        var req = _client.Create("watch/providers/regions");
        if (DefaultLanguage is not null)
        {
            req.AddParameter("language", DefaultLanguage);
        }

        var response = await req.GetOfT<ResultContainer<WatchProviderRegion>>(cancellationToken).ConfigureAwait(false);

        return response;
    }

    /// <summary>
    /// Gets the watch providers (OTT/streaming) available for movies.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The movie watch providers.</returns>
    /// <remarks>Uses <see cref="DefaultCountry"/> and <see cref="DefaultLanguage"/> to filter or translate data.</remarks>
    public async Task<ResultContainer<WatchProviderItem>?> GetMovieWatchProvidersAsync(CancellationToken cancellationToken = default)
    {
        var req = _client.Create("watch/providers/movie");
        if (DefaultLanguage is not null)
        {
            req.AddParameter("language", DefaultLanguage);
        }

        if (DefaultCountry is not null)
        {
            req.AddParameter("watch_region", DefaultCountry);
        }

        var response = await req.GetOfT<ResultContainer<WatchProviderItem>>(cancellationToken).ConfigureAwait(false);

        return response;
    }

    /// <summary>
    /// Gets the watch providers (OTT/streaming) available for TV shows.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The TV watch providers.</returns>
    /// <remarks>Uses <see cref="DefaultCountry"/> and <see cref="DefaultLanguage"/> to filter or translate data.</remarks>
    public async Task<ResultContainer<WatchProviderItem>?> GetTvWatchProvidersAsync(CancellationToken cancellationToken = default)
    {
        var req = _client.Create("watch/providers/tv");
        if (DefaultLanguage is not null)
        {
            req.AddParameter("language", DefaultLanguage);
        }

        if (DefaultCountry is not null)
        {
            req.AddParameter("watch_region", DefaultCountry);
        }

        var response = await req.GetOfT<ResultContainer<WatchProviderItem>>(cancellationToken).ConfigureAwait(false);

        return response;
    }
}
