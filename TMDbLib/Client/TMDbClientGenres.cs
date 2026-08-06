using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Genres;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    /// <summary>
    /// Gets the list of movie genres.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The movie genres.</returns>
    public async Task<List<Genre>?> GetMovieGenresAsync(CancellationToken cancellationToken = default)
    {
        return await GetMovieGenresAsync(DefaultLanguage, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the list of movie genres in a specific language.
    /// </summary>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The movie genres.</returns>
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
    /// Gets the list of TV show genres.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The TV genres.</returns>
    public async Task<List<Genre>?> GetTvGenresAsync(CancellationToken cancellationToken = default)
    {
        return await GetTvGenresAsync(DefaultLanguage, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the list of TV show genres in a specific language.
    /// </summary>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The TV genres.</returns>
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
