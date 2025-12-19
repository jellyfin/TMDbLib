using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.Trending;
using TMDbLib.Rest;
using TMDbLib.Utilities;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    /// <summary>
    /// Retrieves trending movies for a specific time window.
    /// </summary>
    /// <param name="timeWindow">The time window for trending results (day or week).</param>
    /// <param name="page">The page number of results to retrieve.</param>
    /// <param name="language">Language to localize the results in.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A search container with trending movies.</returns>
    public async Task<SearchContainer<SearchMovie>> GetTrendingMoviesAsync(TimeWindow timeWindow, int page = 0, string language = null, CancellationToken cancellationToken = default)
    {
        RestRequest req = _client.Create("trending/movie/{time_window}");
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

        SearchContainer<SearchMovie> resp = await req.GetOfT<SearchContainer<SearchMovie>>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Retrieves trending TV shows for a specific time window.
    /// </summary>
    /// <param name="timeWindow">The time window for trending results (day or week).</param>
    /// <param name="page">The page number of results to retrieve.</param>
    /// <param name="language">Language to localize the results in.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A search container with trending TV shows.</returns>
    public async Task<SearchContainer<SearchTv>> GetTrendingTvAsync(TimeWindow timeWindow, int page = 0, string language = null, CancellationToken cancellationToken = default)
    {
        RestRequest req = _client.Create("trending/tv/{time_window}");
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

        SearchContainer<SearchTv> resp = await req.GetOfT<SearchContainer<SearchTv>>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Retrieves trending people for a specific time window.
    /// </summary>
    /// <param name="timeWindow">The time window for trending results (day or week).</param>
    /// <param name="page">The page number of results to retrieve.</param>
    /// <param name="language">Language to localize the results in.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A search container with trending people.</returns>
    public async Task<SearchContainer<SearchPerson>> GetTrendingPeopleAsync(TimeWindow timeWindow, int page = 0, string language = null, CancellationToken cancellationToken = default)
    {
        RestRequest req = _client.Create("trending/person/{time_window}");
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

        SearchContainer<SearchPerson> resp = await req.GetOfT<SearchContainer<SearchPerson>>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Retrieves all trending items (movies, TV shows, and people) for a specific time window.
    /// </summary>
    /// <param name="timeWindow">The time window for trending results (day or week).</param>
    /// <param name="page">The page number of results to retrieve.</param>
    /// <param name="language">Language to localize the results in.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A search container with all trending items.</returns>
    public async Task<SearchContainer<SearchBase>> GetTrendingAllAsync(TimeWindow timeWindow, int page = 0, string language = null, CancellationToken cancellationToken = default)
    {
        RestRequest req = _client.Create("trending/all/{time_window}");
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

        SearchContainer<SearchBase> resp = await req.GetOfT<SearchContainer<SearchBase>>(cancellationToken).ConfigureAwait(false);

        return resp;
    }
}
