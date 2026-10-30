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
    /// <param name="language">The ISO 639-1 language code used to translate the data. Defaults to <see cref="DefaultLanguage"/>.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The available regions.</returns>
    public async Task<ResultContainer<WatchProviderRegion>?> GetWatchProviderRegionsAsync(string? language = null, CancellationToken cancellationToken = default)
    {
        var req = _client.Create("watch/providers/regions");

        language ??= DefaultLanguage;
        if (!string.IsNullOrWhiteSpace(language))
        {
            req.AddParameter("language", language);
        }

        var response = await req.GetOfT<ResultContainer<WatchProviderRegion>>(cancellationToken).ConfigureAwait(false);

        return response;
    }

    /// <summary>
    /// Gets the watch providers (OTT/streaming) available for movies.
    /// </summary>
    /// <param name="language">The ISO 639-1 language code used to translate the data. Defaults to <see cref="DefaultLanguage"/>.</param>
    /// <param name="watchRegion">The ISO 3166-1 country code to filter by. Defaults to <see cref="DefaultCountry"/>.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The movie watch providers.</returns>
    public async Task<ResultContainer<WatchProviderItem>?> GetMovieWatchProvidersAsync(string? language = null, string? watchRegion = null, CancellationToken cancellationToken = default)
    {
        return await GetWatchProvidersInternal("watch/providers/movie", language, watchRegion, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the watch providers (OTT/streaming) available for TV shows.
    /// </summary>
    /// <param name="language">The ISO 639-1 language code used to translate the data. Defaults to <see cref="DefaultLanguage"/>.</param>
    /// <param name="watchRegion">The ISO 3166-1 country code to filter by. Defaults to <see cref="DefaultCountry"/>.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The TV watch providers.</returns>
    public async Task<ResultContainer<WatchProviderItem>?> GetTvWatchProvidersAsync(string? language = null, string? watchRegion = null, CancellationToken cancellationToken = default)
    {
        return await GetWatchProvidersInternal("watch/providers/tv", language, watchRegion, cancellationToken).ConfigureAwait(false);
    }

    private async Task<ResultContainer<WatchProviderItem>?> GetWatchProvidersInternal(string endpoint, string? language, string? watchRegion, CancellationToken cancellationToken)
    {
        var req = _client.Create(endpoint);

        language ??= DefaultLanguage;
        if (!string.IsNullOrWhiteSpace(language))
        {
            req.AddParameter("language", language);
        }

        watchRegion ??= DefaultCountry;
        if (!string.IsNullOrWhiteSpace(watchRegion))
        {
            req.AddParameter("watch_region", watchRegion);
        }

        return await req.GetOfT<ResultContainer<WatchProviderItem>>(cancellationToken).ConfigureAwait(false);
    }
}
