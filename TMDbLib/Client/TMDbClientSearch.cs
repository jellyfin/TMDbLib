using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Rest;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    private async Task<T?> SearchMethodInternal<T>(string method, string query, int page, string? language = null, bool? includeAdult = null, int year = 0, string? dateFormat = null, string? region = null, int primaryReleaseYear = 0, int firstAirDateYear = 0, CancellationToken cancellationToken = default)
        where T : new()
    {
        var req = _client.Create("search/{method}");
        req.AddUrlSegment("method", method);
        req.AddParameter("query", query);

        language ??= DefaultLanguage;
        if (!string.IsNullOrWhiteSpace(language))
        {
            req.AddParameter("language", language);
        }

        if (page >= 1)
        {
            req.AddParameter("page", page.ToString(CultureInfo.InvariantCulture));
        }

        if (year >= 1)
        {
            req.AddParameter("year", year.ToString(CultureInfo.InvariantCulture));
        }

        if (includeAdult.HasValue)
        {
            req.AddParameter("include_adult", includeAdult.Value ? "true" : "false");
        }

        // TODO: Dateformat?
        // if (dateFormat is not null)
        //    req.DateFormat = dateFormat;

        if (!string.IsNullOrWhiteSpace(region))
        {
            req.AddParameter("region", region);
        }

        if (primaryReleaseYear >= 1)
        {
            req.AddParameter("primary_release_year", primaryReleaseYear.ToString(CultureInfo.InvariantCulture));
        }

        if (firstAirDateYear >= 1)
        {
            req.AddParameter("first_air_date_year", firstAirDateYear.ToString(CultureInfo.InvariantCulture));
        }

        var resp = await req.GetOfT<T>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Searches for collections using the default language.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="page">The page number of results to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A search container with collection results.</returns>
    public async Task<SearchContainer<SearchCollection>?> SearchCollectionAsync(string query, int page = 0, CancellationToken cancellationToken = default)
    {
        return await SearchCollectionAsync(query, DefaultLanguage, page, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Searches for collections with specific language settings.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="language">Language to localize the results in.</param>
    /// <param name="page">The page number of results to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A search container with collection results.</returns>
    public async Task<SearchContainer<SearchCollection>?> SearchCollectionAsync(string query, string? language, int page = 0, CancellationToken cancellationToken = default)
    {
        return await SearchMethodInternal<SearchContainer<SearchCollection>>("collection", query, page, language, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Searches for companies.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="page">The page number of results to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A search container with company results.</returns>
    public async Task<SearchContainer<SearchCompany>?> SearchCompanyAsync(string query, int page = 0, CancellationToken cancellationToken = default)
    {
        return await SearchMethodInternal<SearchContainer<SearchCompany>>("company", query, page, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Searches for keywords.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="page">The page number of results to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A search container with keyword results.</returns>
    public async Task<SearchContainer<SearchKeyword>?> SearchKeywordAsync(string query, int page = 0, CancellationToken cancellationToken = default)
    {
        return await SearchMethodInternal<SearchContainer<SearchKeyword>>("keyword", query, page, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Searches for movies using the default language.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="page">The page number of results to retrieve (defaults to 1).</param>
    /// <param name="includeAdult">Whether to include adult movies in the results.</param>
    /// <param name="year">The year to filter results by.</param>
    /// <param name="region">The region to limit results to.</param>
    /// <param name="primaryReleaseYear">The primary release year to filter by.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A search container with movie results.</returns>
    public async Task<SearchContainer<SearchMovie>?> SearchMovieAsync(string query, int page = 0, bool includeAdult = false, int year = 0, string? region = null, int primaryReleaseYear = 0, CancellationToken cancellationToken = default)
    {
        return await SearchMovieAsync(query, DefaultLanguage, page, includeAdult, year, region, primaryReleaseYear, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Searches for movies with specific language settings.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="language">The language to localize results in.</param>
    /// <param name="page">The page number of results to retrieve (defaults to 1).</param>
    /// <param name="includeAdult">Whether to include adult movies in the results.</param>
    /// <param name="year">The year to filter results by.</param>
    /// <param name="region">The region to limit results to.</param>
    /// <param name="primaryReleaseYear">The primary release year to filter by.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A search container with movie results.</returns>
    public async Task<SearchContainer<SearchMovie>?> SearchMovieAsync(string query, string? language, int page = 0, bool includeAdult = false, int year = 0, string? region = null, int primaryReleaseYear = 0, CancellationToken cancellationToken = default)
    {
        return await SearchMethodInternal<SearchContainer<SearchMovie>>("movie", query, page, language, includeAdult, year, "yyyy-MM-dd", region, primaryReleaseYear, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Searches for movies, TV shows, and people in a single request using the default language.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="page">The page number of results to retrieve.</param>
    /// <param name="includeAdult">Whether to include adult content in the results.</param>
    /// <param name="year">The year to filter results by.</param>
    /// <param name="region">The region to limit results to.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A search container with mixed results including movies, TV shows, and people.</returns>
    public async Task<SearchContainer<SearchBase>?> SearchMultiAsync(string query, int page = 0, bool includeAdult = false, int year = 0, string? region = null, CancellationToken cancellationToken = default)
    {
        return await SearchMultiAsync(query, DefaultLanguage, page, includeAdult, year, region, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Searches for movies, TV shows, and people in a single request with specific language settings.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="language">Language to localize the results in.</param>
    /// <param name="page">The page number of results to retrieve.</param>
    /// <param name="includeAdult">Whether to include adult content in the results.</param>
    /// <param name="year">The year to filter results by.</param>
    /// <param name="region">The region to limit results to.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A search container with mixed results including movies, TV shows, and people.</returns>
    public async Task<SearchContainer<SearchBase>?> SearchMultiAsync(string query, string? language, int page = 0, bool includeAdult = false, int year = 0, string? region = null, CancellationToken cancellationToken = default)
    {
        return await SearchMethodInternal<SearchContainer<SearchBase>>("multi", query, page, language, includeAdult, year, "yyyy-MM-dd", region, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Searches for people using the default language.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="page">The page number of results to retrieve.</param>
    /// <param name="includeAdult">Whether to include adult content in the results.</param>
    /// <param name="region">The region to limit results to.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A search container with person results.</returns>
    public async Task<SearchContainer<SearchPerson>?> SearchPersonAsync(string query, int page = 0, bool includeAdult = false, string? region = null, CancellationToken cancellationToken = default)
    {
        return await SearchPersonAsync(query, DefaultLanguage, page, includeAdult, region, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Searches for people with specific language settings.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="language">Language to localize the results in.</param>
    /// <param name="page">The page number of results to retrieve.</param>
    /// <param name="includeAdult">Whether to include adult content in the results.</param>
    /// <param name="region">The region to limit results to.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A search container with person results.</returns>
    public async Task<SearchContainer<SearchPerson>?> SearchPersonAsync(string query, string? language, int page = 0, bool includeAdult = false, string? region = null, CancellationToken cancellationToken = default)
    {
        return await SearchMethodInternal<SearchContainer<SearchPerson>>("person", query, page, language, includeAdult, region: region, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Searches for TV shows using the default language.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="page">The page number of results to retrieve.</param>
    /// <param name="includeAdult">Whether to include adult content in the results.</param>
    /// <param name="firstAirDateYear">The first air date year to filter by.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A search container with TV show results.</returns>
    public async Task<SearchContainer<SearchTv>?> SearchTvShowAsync(string query, int page = 0, bool includeAdult = false, int firstAirDateYear = 0, CancellationToken cancellationToken = default)
    {
        return await SearchTvShowAsync(query, DefaultLanguage, page, includeAdult, firstAirDateYear, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Searches for TV shows with specific language settings.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="language">Language to localize the results in.</param>
    /// <param name="page">The page number of results to retrieve.</param>
    /// <param name="includeAdult">Whether to include adult content in the results.</param>
    /// <param name="firstAirDateYear">The first air date year to filter by.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A search container with TV show results.</returns>
    public async Task<SearchContainer<SearchTv>?> SearchTvShowAsync(string query, string? language, int page = 0, bool includeAdult = false, int firstAirDateYear = 0, CancellationToken cancellationToken = default)
    {
        return await SearchMethodInternal<SearchContainer<SearchTv>>("tv", query, page, language, includeAdult, firstAirDateYear: firstAirDateYear, cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}
