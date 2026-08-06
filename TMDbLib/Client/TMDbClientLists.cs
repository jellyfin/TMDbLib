using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Lists;
using TMDbLib.Rest;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    private async Task<bool> GetManipulateMediaListAsyncInternal(string listId, int movieId, string method, CancellationToken cancellationToken = default)
    {
        RequireSessionId(SessionType.UserSession);

        if (string.IsNullOrWhiteSpace(listId))
        {
            throw new ArgumentNullException(nameof(listId));
        }

        // Movie Id is expected by the API and can not be null
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(movieId);

        RestRequest req = _client.Create("list/{listId}/{method}");
        req.AddUrlSegment("listId", listId);
        req.AddUrlSegment("method", method);
        AddSessionId(req, SessionType.UserSession);

        req.SetBody(new { media_id = movieId });

        using RestResponse<PostReply> response = await req.Post<PostReply>(cancellationToken).ConfigureAwait(false);

        // Status code 12 = "The item/record was updated successfully"
        // Status code 13 = "The item/record was deleted successfully"
        var item = await response.GetDataObject().ConfigureAwait(false);

        return item is not null && (item.StatusCode == 12 || item.StatusCode == 13);
    }

    /// <summary>
    /// Gets a list by id.
    /// </summary>
    /// <param name="listId">The id of the list.</param>
    /// <param name="language">The ISO 639-1 language code (e.g. en, it, es).</param>
    /// <param name="page">The page number (starting at 1).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The list with its items.</returns>
    public async Task<GenericList?> GetListAsync(string listId, string? language = null, int page = 0, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(listId))
        {
            throw new ArgumentNullException(nameof(listId));
        }

        RestRequest req = _client.Create("list/{listId}");
        req.AddUrlSegment("listId", listId);

        language ??= DefaultLanguage;
        if (!string.IsNullOrWhiteSpace(language))
        {
            req.AddParameter("language", language);
        }

        if (page >= 1)
        {
            req.AddParameter("page", page.ToString(CultureInfo.InvariantCulture));
        }

        var resp = await req.GetOfT<GenericList>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Checks whether a movie is present in a list.
    /// </summary>
    /// <param name="listId">The id of the list.</param>
    /// <param name="movieId">The id of the movie.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the movie is in the list.</returns>
    public async Task<bool> GetListIsMoviePresentAsync(string listId, int movieId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(listId))
        {
            throw new ArgumentNullException(nameof(listId));
        }

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(movieId);

        var req = _client.Create("list/{listId}/item_status");
        req.AddUrlSegment("listId", listId);
        req.AddParameter("movie_id", movieId.ToString(CultureInfo.InvariantCulture));

        using var response = await req.Get<ListStatus>(cancellationToken).ConfigureAwait(false);
        var item = await response.GetDataObject().ConfigureAwait(false);

        return item?.ItemPresent ?? false;
    }

    /// <summary>
    /// Adds a movie to a list.
    /// </summary>
    /// <param name="listId">The id of the list.</param>
    /// <param name="movieId">The id of the movie to add.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the movie was added; false if it was already in the list or on failure.</returns>
    /// <remarks>Requires a valid user session.</remarks>
    /// <exception cref="UserSessionRequiredException">Thrown when no user session is assigned.</exception>
    public async Task<bool> ListAddMovieAsync(string listId, int movieId, CancellationToken cancellationToken = default)
    {
        return await GetManipulateMediaListAsyncInternal(listId, movieId, "add_item", cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Clears all items from a list without confirmation.
    /// </summary>
    /// <param name="listId">The id of the list.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the list was cleared.</returns>
    /// <remarks>Requires a valid user session.</remarks>
    /// <exception cref="UserSessionRequiredException">Thrown when no user session is assigned.</exception>
    public async Task<bool> ListClearAsync(string listId, CancellationToken cancellationToken = default)
    {
        RequireSessionId(SessionType.UserSession);

        if (string.IsNullOrWhiteSpace(listId))
        {
            throw new ArgumentNullException(nameof(listId));
        }

        var request = _client.Create("list/{listId}/clear");
        request.AddUrlSegment("listId", listId);
        request.AddParameter("confirm", "true");
        AddSessionId(request, SessionType.UserSession);

        using var response = await request.Post<PostReply>(cancellationToken).ConfigureAwait(false);

        // Status code 12 = "The item/record was updated successfully"
        var item = await response.GetDataObject().ConfigureAwait(false);

        return item is not null && item.StatusCode == 12;
    }

    /// <summary>
    /// Creates a new list for the current user.
    /// </summary>
    /// <param name="name">The name of the new list.</param>
    /// <param name="description">Optional description.</param>
    /// <param name="language">Optional ISO 639-1 language code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The id of the new list.</returns>
    /// <remarks>Requires a valid user session.</remarks>
    /// <exception cref="UserSessionRequiredException">Thrown when no user session is assigned.</exception>
    public async Task<string?> ListCreateAsync(string name, string description = "", string? language = null, CancellationToken cancellationToken = default)
    {
        RequireSessionId(SessionType.UserSession);

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        // Description is expected by the API and can not be null
        if (string.IsNullOrWhiteSpace(description))
        {
            description = string.Empty;
        }

        var req = _client.Create("list");
        AddSessionId(req, SessionType.UserSession);

        language ??= DefaultLanguage;
        if (!string.IsNullOrWhiteSpace(language))
        {
            req.SetBody(new { name, description, language });
        }
        else
        {
            req.SetBody(new { name, description });
        }

        using var response = await req.Post<ListCreateReply>(cancellationToken).ConfigureAwait(false);
        var item = await response.GetDataObject().ConfigureAwait(false);

        return item?.ListId;
    }

    /// <summary>
    /// Deletes a list owned by the current user.
    /// </summary>
    /// <param name="listId">The id of the list.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the list was deleted.</returns>
    /// <remarks>Requires a valid user session.</remarks>
    /// <exception cref="UserSessionRequiredException">Thrown when no user session is assigned.</exception>
    public async Task<bool> ListDeleteAsync(string listId, CancellationToken cancellationToken = default)
    {
        RequireSessionId(SessionType.UserSession);

        if (string.IsNullOrWhiteSpace(listId))
        {
            throw new ArgumentNullException(nameof(listId));
        }

        var req = _client.Create("list/{listId}");
        req.AddUrlSegment("listId", listId);
        AddSessionId(req, SessionType.UserSession);

        using var response = await req.Delete<PostReply>(cancellationToken).ConfigureAwait(false);

        // Status code 13 = success
        var item = await response.GetDataObject().ConfigureAwait(false);

        return item is not null && item.StatusCode == 13;
    }

    /// <summary>
    /// Removes a movie from a list.
    /// </summary>
    /// <param name="listId">The id of the list.</param>
    /// <param name="movieId">The id of the movie to remove.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the movie was removed; false if it wasn't in the list or on failure.</returns>
    /// <remarks>Requires a valid user session.</remarks>
    /// <exception cref="UserSessionRequiredException">Thrown when no user session is assigned.</exception>
    public async Task<bool> ListRemoveMovieAsync(string listId, int movieId, CancellationToken cancellationToken = default)
    {
        return await GetManipulateMediaListAsyncInternal(listId, movieId, "remove_item", cancellationToken).ConfigureAwait(false);
    }
}
