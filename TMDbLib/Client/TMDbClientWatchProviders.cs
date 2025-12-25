using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Rest;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    /// <summary>
    /// Returns a list of all of the countries TMDb has watch provider (OTT/streaming) data for.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A list of regions/countries with watch provider data available.</returns>
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
    /// Returns a list of the watch provider (OTT/streaming) data TMDb has available for movies.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A list of watch providers for movies.</returns>
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
    /// Returns a list of the watch provider (OTT/streaming) data TMDb has available for shows.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A list of watch providers for TV shows.</returns>
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
