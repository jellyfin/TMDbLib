using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Rest;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    /// <summary>
    /// Retrieves a keyword by its TMDb ID.
    /// </summary>
    /// <param name="keywordId">The TMDb ID of the keyword.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The keyword object with ID and name.</returns>
    public async Task<Keyword> GetKeywordAsync(int keywordId, CancellationToken cancellationToken = default)
    {
        RestRequest req = _client.Create("keyword/{keywordId}");
        req.AddUrlSegment("keywordId", keywordId.ToString(CultureInfo.InvariantCulture));

        Keyword resp = await req.GetOfT<Keyword>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Retrieves a paginated list of movies tagged with a specific keyword.
    /// </summary>
    /// <param name="keywordId">The TMDb ID of the keyword.</param>
    /// <param name="page">The page of results to retrieve. Use 0 for the default page.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A search container with movies tagged with the specified keyword.</returns>
    public async Task<SearchContainerWithId<SearchMovie>> GetKeywordMoviesAsync(int keywordId, int page = 0, CancellationToken cancellationToken = default)
    {
        return await GetKeywordMoviesAsync(keywordId, DefaultLanguage, page, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a paginated list of movies tagged with a specific keyword with language option.
    /// </summary>
    /// <param name="keywordId">The TMDb ID of the keyword.</param>
    /// <param name="language">The ISO 639-1 language code for movie text. Defaults to the client's DefaultLanguage.</param>
    /// <param name="page">The page of results to retrieve. Use 0 for the default page.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A search container with movies tagged with the specified keyword.</returns>
    public async Task<SearchContainerWithId<SearchMovie>> GetKeywordMoviesAsync(int keywordId, string language, int page = 0, CancellationToken cancellationToken = default)
    {
        RestRequest req = _client.Create("keyword/{keywordId}/movies");
        req.AddUrlSegment("keywordId", keywordId.ToString(CultureInfo.InvariantCulture));

        language ??= DefaultLanguage;
        if (!string.IsNullOrWhiteSpace(language))
        {
            req.AddParameter("language", language);
        }

        if (page >= 1)
        {
            req.AddParameter("page", page.ToString(CultureInfo.InvariantCulture));
        }

        SearchContainerWithId<SearchMovie> resp = await req.GetOfT<SearchContainerWithId<SearchMovie>>(cancellationToken).ConfigureAwait(false);

        return resp;
    }
}
