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
    /// Gets a keyword by id.
    /// </summary>
    /// <param name="keywordId">The TMDb id of the keyword.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The keyword.</returns>
    public async Task<Keyword?> GetKeywordAsync(int keywordId, CancellationToken cancellationToken = default)
    {
        var req = _client.Create("keyword/{keywordId}");
        req.AddUrlSegment("keywordId", keywordId.ToString(CultureInfo.InvariantCulture));

        var resp = await req.GetOfT<Keyword>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Gets the movies tagged with a keyword.
    /// </summary>
    /// <param name="keywordId">The TMDb id of the keyword.</param>
    /// <param name="page">The page number. Use 0 for the default.</param>
    /// <param name="includeAdult">Whether to include adult movies.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The movies tagged with the keyword.</returns>
    public async Task<SearchContainerWithId<SearchMovie>?> GetKeywordMoviesAsync(int keywordId, int page = 0, bool includeAdult = false, CancellationToken cancellationToken = default)
    {
        return await GetKeywordMoviesAsync(keywordId, DefaultLanguage, page, includeAdult, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the movies tagged with a keyword in a specific language.
    /// </summary>
    /// <param name="keywordId">The TMDb id of the keyword.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="page">The page number. Use 0 for the default.</param>
    /// <param name="includeAdult">Whether to include adult movies.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The movies tagged with the keyword.</returns>
    public async Task<SearchContainerWithId<SearchMovie>?> GetKeywordMoviesAsync(int keywordId, string? language, int page = 0, bool includeAdult = false, CancellationToken cancellationToken = default)
    {
        var req = _client.Create("keyword/{keywordId}/movies");
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

        req.AddParameter("include_adult", includeAdult ? "true" : "false");

        var resp = await req.GetOfT<SearchContainerWithId<SearchMovie>>(cancellationToken).ConfigureAwait(false);

        return resp;
    }
}
