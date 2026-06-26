using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.General;
using TMDbLib.Rest;
using TMDbLib.Utilities;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    /// <summary>
    /// Creates a builder for discovering movies matching certain criteria.
    /// </summary>
    /// <returns>A configurable discovery query.</returns>
    public DiscoverMovie DiscoverMoviesAsync()
    {
        return new DiscoverMovie(this);
    }

    internal async Task<SearchContainer<T>?> DiscoverPerformAsync<T>(string endpoint, string? language, int page, SimpleNamedValueCollection parameters, CancellationToken cancellationToken = default)
    {
        var request = _client.Create(endpoint);

        if (page != 1 && page > 1)
        {
            request.AddParameter("page", page.ToString(CultureInfo.InvariantCulture));
        }

        if (!string.IsNullOrWhiteSpace(language))
        {
            request.AddParameter("language", language);
        }

        foreach (var pair in parameters)
        {
            request.AddParameter(pair.Key, pair.Value);
        }

        var response = await request.GetOfT<SearchContainer<T>>(cancellationToken).ConfigureAwait(false);
        return response;
    }

    /// <summary>
    /// Creates a builder for discovering TV shows matching certain criteria.
    /// </summary>
    /// <returns>A configurable discovery query.</returns>
    public DiscoverTv DiscoverTvShowsAsync()
    {
        return new DiscoverTv(this);
    }
}
