using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Genres;
using TMDbLib.Rest;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    /// <summary>
    /// Retrieves a list of all movie genres available on TMDb.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A list of movie genres with IDs and names.</returns>
    public async Task<List<Genre>?> GetMovieGenresAsync(CancellationToken cancellationToken = default)
    {
        return await GetMovieGenresAsync(DefaultLanguage, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a list of all movie genres available on TMDb with language option.
    /// </summary>
    /// <param name="language">The ISO 639-1 language code for genre names. Defaults to the client's DefaultLanguage.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A list of movie genres with IDs and names.</returns>
    public async Task<List<Genre>?> GetMovieGenresAsync(string? language, CancellationToken cancellationToken = default)
    {
        var req = _client.Create("genre/movie/list");

        language ??= DefaultLanguage;
        if (!string.IsNullOrWhiteSpace(language))
        {
            req.AddParameter("language", language);
        }

        using var resp = await req.Get<GenreContainer>(cancellationToken).ConfigureAwait(false);
        var container = await resp.GetDataObject().ConfigureAwait(false);

        return container?.Genres;
    }

    /// <summary>
    /// Retrieves a list of all TV show genres available on TMDb.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A list of TV genres with IDs and names.</returns>
    public async Task<List<Genre>?> GetTvGenresAsync(CancellationToken cancellationToken = default)
    {
        return await GetTvGenresAsync(DefaultLanguage, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a list of all TV show genres available on TMDb with language option.
    /// </summary>
    /// <param name="language">The ISO 639-1 language code for genre names. Defaults to the client's DefaultLanguage.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A list of TV genres with IDs and names.</returns>
    public async Task<List<Genre>?> GetTvGenresAsync(string? language, CancellationToken cancellationToken = default)
    {
        var req = _client.Create("genre/tv/list");

        language ??= DefaultLanguage;
        if (!string.IsNullOrWhiteSpace(language))
        {
            req.AddParameter("language", language);
        }

        using var resp = await req.Get<GenreContainer>(cancellationToken).ConfigureAwait(false);
        var container = await resp.GetDataObject().ConfigureAwait(false);

        return container?.Genres;
    }
}
