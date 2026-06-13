using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.Find;
using TMDbLib.Rest;
using TMDbLib.Utilities;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    /// <summary>
    /// Finds movies, people, and TV shows by an external id.
    /// </summary>
    /// <param name="source">The external source.</param>
    /// <param name="id">The external id to look up.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>All matching TMDb objects.</returns>
    /// <remarks>
    /// Supported sources: Movies (Imdb); People (Imdb, FreeBaseMid, FreeBaseId, TvRage);
    /// TV Series (Imdb, FreeBaseMid, FreeBaseId, TvRage, TvDb).
    /// </remarks>
    public Task<FindContainer?> FindAsync(FindExternalSource source, string id, CancellationToken cancellationToken = default)
    {
        return FindAsync(source, id, null, cancellationToken);
    }

    /// <summary>
    /// Finds movies, people, and TV shows by an external id in a specific language.
    /// </summary>
    /// <param name="source">The external source.</param>
    /// <param name="id">The external id to look up.</param>
    /// <param name="language">The ISO 639-1 language code (e.g. en, it, es).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>All matching TMDb objects.</returns>
    /// <remarks>
    /// Supported sources: Movies (Imdb); People (Imdb, FreeBaseMid, FreeBaseId, TvRage);
    /// TV Series (Imdb, FreeBaseMid, FreeBaseId, TvRage, TvDb).
    /// </remarks>
    public async Task<FindContainer?> FindAsync(FindExternalSource source, string id, string? language, CancellationToken cancellationToken = default)
    {
        var req = _client.Create("find/{id}");

        req.AddUrlSegment("id", WebUtility.UrlEncode(id));
        req.AddParameter("external_source", source.GetDescription());

        language ??= DefaultLanguage;
        if (!string.IsNullOrEmpty(language))
        {
            req.AddParameter("language", language);
        }

        var resp = await req.GetOfT<FindContainer>(cancellationToken).ConfigureAwait(false);

        return resp;
    }
}
