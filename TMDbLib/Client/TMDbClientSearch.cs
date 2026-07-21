using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.General.Schema;
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
    /// Searches for collections.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="page">The page number.</param>
    /// <param name="includeAdult">Whether to include adult collections.</param>
    /// <param name="region">The ISO 3166-1 region code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The matching collections.</returns>
    public async Task<SearchContainer<SearchCollection>?> SearchCollectionAsync(string query, int page = 0, bool includeAdult = false, string? region = null, CancellationToken cancellationToken = default)
    {
        return await SearchCollectionAsync(query, DefaultLanguage, page, includeAdult, region, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Searches for collections in a specific language.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="page">The page number.</param>
    /// <param name="includeAdult">Whether to include adult collections.</param>
    /// <param name="region">The ISO 3166-1 region code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The matching collections.</returns>
    public async Task<SearchContainer<SearchCollection>?> SearchCollectionAsync(string query, string? language, int page = 0, bool includeAdult = false, string? region = null, CancellationToken cancellationToken = default)
    {
        return await SearchMethodInternal<SearchContainer<SearchCollection>>("collection", query, page, language, includeAdult, region: region, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Searches for companies.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="page">The page number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The matching companies.</returns>
    public async Task<SearchContainer<SearchCompany>?> SearchCompanyAsync(string query, int page = 0, CancellationToken cancellationToken = default)
    {
        return await SearchMethodInternal<SearchContainer<SearchCompany>>("company", query, page, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Searches for keywords.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="page">The page number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The matching keywords.</returns>
    public async Task<SearchContainer<SearchKeyword>?> SearchKeywordAsync(string query, int page = 0, CancellationToken cancellationToken = default)
    {
        return await SearchMethodInternal<SearchContainer<SearchKeyword>>("keyword", query, page, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Searches for movies.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="page">The page number.</param>
    /// <param name="includeAdult">Whether to include adult movies.</param>
    /// <param name="year">The year filter.</param>
    /// <param name="region">The ISO 3166-1 region code.</param>
    /// <param name="primaryReleaseYear">The primary release year filter.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The matching movies.</returns>
    public async Task<SearchContainer<SearchMovie>?> SearchMovieAsync(string query, int page = 0, bool includeAdult = false, int year = 0, string? region = null, int primaryReleaseYear = 0, CancellationToken cancellationToken = default)
    {
        return await SearchMovieAsync(query, DefaultLanguage, page, includeAdult, year, region, primaryReleaseYear, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Searches for movies in a specific language.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="page">The page number.</param>
    /// <param name="includeAdult">Whether to include adult movies.</param>
    /// <param name="year">The year filter.</param>
    /// <param name="region">The ISO 3166-1 region code.</param>
    /// <param name="primaryReleaseYear">The primary release year filter.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The matching movies.</returns>
    public async Task<SearchContainer<SearchMovie>?> SearchMovieAsync(string query, string? language, int page = 0, bool includeAdult = false, int year = 0, string? region = null, int primaryReleaseYear = 0, CancellationToken cancellationToken = default)
    {
        return await SearchMethodInternal<SearchContainer<SearchMovie>>("movie", query, page, language, includeAdult, year, region: region, primaryReleaseYear: primaryReleaseYear, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Searches movies, TV shows, and people in a single request.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="page">The page number.</param>
    /// <param name="includeAdult">Whether to include adult content.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Mixed results of movies, TV shows, and people.</returns>
    public async Task<SearchContainer<TmdbEntity>?> SearchMultiAsync(string query, int page = 0, bool includeAdult = false, CancellationToken cancellationToken = default)
    {
        return await SearchMultiAsync(query, DefaultLanguage, page, includeAdult, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Searches movies, TV shows, and people in a specific language.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="page">The page number.</param>
    /// <param name="includeAdult">Whether to include adult content.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Mixed results of movies, TV shows, and people.</returns>
    public async Task<SearchContainer<TmdbEntity>?> SearchMultiAsync(string query, string? language, int page = 0, bool includeAdult = false, CancellationToken cancellationToken = default)
    {
        return await SearchMethodInternal<SearchContainer<TmdbEntity>>("multi", query, page, language, includeAdult, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Searches for people.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="page">The page number.</param>
    /// <param name="includeAdult">Whether to include adult content.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The matching people.</returns>
    public async Task<SearchContainer<SearchPerson>?> SearchPersonAsync(string query, int page = 0, bool includeAdult = false, CancellationToken cancellationToken = default)
    {
        return await SearchPersonAsync(query, DefaultLanguage, page, includeAdult, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Searches for people in a specific language.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="page">The page number.</param>
    /// <param name="includeAdult">Whether to include adult content.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The matching people.</returns>
    public async Task<SearchContainer<SearchPerson>?> SearchPersonAsync(string query, string? language, int page = 0, bool includeAdult = false, CancellationToken cancellationToken = default)
    {
        return await SearchMethodInternal<SearchContainer<SearchPerson>>("person", query, page, language, includeAdult, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Searches for TV shows.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="page">The page number.</param>
    /// <param name="includeAdult">Whether to include adult content.</param>
    /// <param name="firstAirDateYear">The first-air-date year filter.</param>
    /// <param name="year">Matches any series or episode air date.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The matching TV shows.</returns>
    public async Task<SearchContainer<SearchTv>?> SearchTvShowAsync(string query, int page = 0, bool includeAdult = false, int firstAirDateYear = 0, int year = 0, CancellationToken cancellationToken = default)
    {
        return await SearchTvShowAsync(query, DefaultLanguage, page, includeAdult, firstAirDateYear, year, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Searches for TV shows in a specific language.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="page">The page number.</param>
    /// <param name="includeAdult">Whether to include adult content.</param>
    /// <param name="firstAirDateYear">The first-air-date year filter.</param>
    /// <param name="year">Matches any series or episode air date.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The matching TV shows.</returns>
    public async Task<SearchContainer<SearchTv>?> SearchTvShowAsync(string query, string? language, int page = 0, bool includeAdult = false, int firstAirDateYear = 0, int year = 0, CancellationToken cancellationToken = default)
    {
        return await SearchMethodInternal<SearchContainer<SearchTv>>("tv", query, page, language, includeAdult, year, firstAirDateYear: firstAirDateYear, cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}
