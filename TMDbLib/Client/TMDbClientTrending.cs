using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.General.Schema;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.Trending;
using TMDbLib.Rest;
using TMDbLib.Utilities;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    /// <summary>
    /// Gets the trending movies for a time window.
    /// </summary>
    /// <param name="timeWindow">The time window (day or week).</param>
    /// <param name="page">The page number.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The trending movies.</returns>
    public async Task<SearchContainer<SearchMovie>?> GetTrendingMoviesAsync(TimeWindow timeWindow, int page = 0, string? language = null, CancellationToken cancellationToken = default)
    {
        var req = _client.Create("trending/movie/{time_window}");
        req.AddUrlSegment("time_window", timeWindow.GetDescription());

        if (page >= 1)
        {
            req.AddQueryString("page", page.ToString(CultureInfo.InvariantCulture));
        }

        if (!string.IsNullOrWhiteSpace(language))
        {
            req.AddQueryString("language", language);
        }
        else if (!string.IsNullOrWhiteSpace(DefaultLanguage))
        {
            req.AddParameter("language", DefaultLanguage);
        }

        var resp = await req.GetOfT<SearchContainer<SearchMovie>>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Gets the trending TV shows for a time window.
    /// </summary>
    /// <param name="timeWindow">The time window (day or week).</param>
    /// <param name="page">The page number.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The trending TV shows.</returns>
    public async Task<SearchContainer<SearchTv>?> GetTrendingTvAsync(TimeWindow timeWindow, int page = 0, string? language = null, CancellationToken cancellationToken = default)
    {
        var req = _client.Create("trending/tv/{time_window}");
        req.AddUrlSegment("time_window", timeWindow.GetDescription());

        if (page >= 1)
        {
            req.AddQueryString("page", page.ToString(CultureInfo.InvariantCulture));
        }

        if (!string.IsNullOrWhiteSpace(language))
        {
            req.AddQueryString("language", language);
        }
        else if (!string.IsNullOrWhiteSpace(DefaultLanguage))
        {
            req.AddParameter("language", DefaultLanguage);
        }

        var resp = await req.GetOfT<SearchContainer<SearchTv>>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Gets the trending people for a time window.
    /// </summary>
    /// <param name="timeWindow">The time window (day or week).</param>
    /// <param name="page">The page number.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The trending people.</returns>
    public async Task<SearchContainer<SearchPerson>?> GetTrendingPeopleAsync(TimeWindow timeWindow, int page = 0, string? language = null, CancellationToken cancellationToken = default)
    {
        var req = _client.Create("trending/person/{time_window}");
        req.AddUrlSegment("time_window", timeWindow.GetDescription());

        if (page >= 1)
        {
            req.AddQueryString("page", page.ToString(CultureInfo.InvariantCulture));
        }

        if (!string.IsNullOrWhiteSpace(language))
        {
            req.AddQueryString("language", language);
        }
        else if (!string.IsNullOrWhiteSpace(DefaultLanguage))
        {
            req.AddParameter("language", DefaultLanguage);
        }

        var resp = await req.GetOfT<SearchContainer<SearchPerson>>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Gets all trending items (movies, TV shows, and people) for a time window.
    /// </summary>
    /// <param name="timeWindow">The time window (day or week).</param>
    /// <param name="page">The page number.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The trending items.</returns>
    public async Task<SearchContainer<TmdbEntity>?> GetTrendingAllAsync(TimeWindow timeWindow, int page = 0, string? language = null, CancellationToken cancellationToken = default)
    {
        var req = _client.Create("trending/all/{time_window}");
        req.AddUrlSegment("time_window", timeWindow.GetDescription());

        if (page >= 1)
        {
            req.AddQueryString("page", page.ToString(CultureInfo.InvariantCulture));
        }

        if (!string.IsNullOrWhiteSpace(language))
        {
            req.AddQueryString("language", language);
        }
        else if (!string.IsNullOrWhiteSpace(DefaultLanguage))
        {
            req.AddParameter("language", DefaultLanguage);
        }

        var resp = await req.GetOfT<SearchContainer<TmdbEntity>>(cancellationToken).ConfigureAwait(false);

        return resp;
    }
}
